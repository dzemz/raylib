#version 330

// Input vertex attributes (from vertex shader)
in vec2 fragTexCoord;

// Input uniform values
uniform sampler2D texture0;

// Output fragment color
out vec4 finalColor;

// NOTE: Add here your custom variables
const vec2 LeftLensCenter = vec2(0.2863248, 0.5);
const vec2 RightLensCenter = vec2(0.7136753, 0.5);
const vec2 LeftScreenCenter = vec2(0.25, 0.5);
const vec2 RightScreenCenter = vec2(0.75, 0.5);
const vec2 Scale = vec2(0.25, 0.45);    //vec2(0.1469278, 0.2350845);
const vec2 ScaleIn = vec2(4, 2.2222);
const vec4 HmdWarpParam = vec4(1, 0.22, 0.24, 0);

/*
// Another set of default values
ChromaAbCorrection = {1.0, 0.0, 1.0, 0}
DistortionK = {1.0, 0.22, 0.24, 0}
Scale = {0.25, 0.5*AspectRatio, 0, 0}
ScaleIn = {4.0, 2/AspectRatio, 0, 0}
Left Screen Center = {0.25, 0.5, 0, 0}
Left Lens Center = {0.287994117, 0.5, 0, 0}
Right Screen Center = {0.75, 0.5, 0, 0}
Right Lens Center = {0.712005913, 0.5, 0, 0}
*/

// Scales input texture coordinates for distortion.
vec2 HmdWarp(vec2 in01, vec2 LensCenter)
{
    vec2 theta = (in01 - LensCenter) * ScaleIn; // Scales to [-1, 1]
    float rSq = theta.x * theta.x + theta.y * theta.y;
    vec2 rvector = theta * (HmdWarpParam.x + HmdWarpParam.y * rSq + HmdWarpParam.z * rSq * rSq + HmdWarpParam.w * rSq * rSq * rSq);

    return LensCenter + Scale * rvector;
}

void main()
{
    // SOURCE: http://www.mtbs3d.com/phpbb/viewtopic.php?f=140&t=17081
    
    // The following two variables need to be set per eye
    vec2 LensCenter = gl_FragCoord.x < 540 ? LeftLensCenter : RightLensCenter;
    vec2 ScreenCenter = gl_FragCoord.x < 540 ? LeftScreenCenter : RightScreenCenter;

    //vec2 oTexCoord = (gl_FragCoord.xy + vec2(0.5, 0.5)) / vec2(1280, 800);  //Uncomment if using BGE's built-in stereo rendering

    vec2 tc = HmdWarp(fragTexCoord, LensCenter);

    if (any(bvec2(clamp(tc,ScreenCenter-vec2(0.25,0.5), ScreenCenter+vec2(0.25,0.5)) - tc)))
    {
        gl_FragColor = vec4(vec3(0.0), 1.0);
    }
    else
    {
        //tc.x = gl_FragCoord.x < 640 ? (2.0 * tc.x) : (2.0 * (tc.x - 0.5));  //Uncomment if using BGE's built-in stereo rendering
        gl_FragColor = texture2D(texture0, tc);
    }
}