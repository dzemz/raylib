#**************************************************************************************************
#
#   raylib - Advance Game
#
#   makefile to compile advance game for desktop platforms, Raspberry Pi and HTML5 (emscripten)
#
#   Copyright (c) 2014 Ramon Santamaria (Ray San - raysan@raysanweb.com)
#    
#   This software is provided "as-is", without any express or implied warranty. In no event 
#   will the authors be held liable for any damages arising from the use of this software.
#
#   Permission is granted to anyone to use this software for any purpose, including commercial 
#   applications, and to alter it and redistribute it freely, subject to the following restrictions:
#
#     1. The origin of this software must not be misrepresented; you must not claim that you 
#     wrote the original software. If you use this software in a product, an acknowledgment 
#     in the product documentation would be appreciated but is not required.
#
#     2. Altered source versions must be plainly marked as such, and must not be misrepresented
#     as being the original software.
#
#     3. This notice may not be removed or altered from any source distribution.
#
#**************************************************************************************************

# define raylib platform to compile for
# possible platforms: PLATFORM_DESKTOP PLATFORM_RPI PLATFORM_WEB
# WARNING: To compile to HTML5, code must be redesigned to use emscripten.h and emscripten_set_main_loop()
PLATFORM ?= PLATFORM_DESKTOP

# determine PLATFORM_OS in case PLATFORM_DESKTOP selected
ifeq ($(PLATFORM),PLATFORM_DESKTOP)
    # No uname.exe on MinGW!, but OS=Windows_NT on Windows! ifeq ($(UNAME),Msys) -> Windows
    ifeq ($(OS),Windows_NT)
        PLATFORM_OS=WINDOWS
        LIBPATH=win32
    else
        UNAMEOS:=$(shell uname)
        ifeq ($(UNAMEOS),Linux)
            PLATFORM_OS=LINUX
            LIBPATH=linux
        else
        ifeq ($(UNAMEOS),Darwin)
            PLATFORM_OS=OSX
            LIBPATH=osx
        endif
        endif
    endif
endif

# define compiler: gcc for C program, define as g++ for C++
ifeq ($(PLATFORM),PLATFORM_WEB)
    # define emscripten compiler
    CC = emcc
else
ifeq ($(PLATFORM_OS),OSX)
    # define llvm compiler for mac
    CC = clang
else
    # define default gcc compiler
    CC = gcc
endif
endif

# define compiler flags:
#  -O2         defines optimization level
#  -Wall       turns on most, but not all, compiler warnings
#  -std=c99    use standard C from 1999 revision
ifeq ($(PLATFORM),PLATFORM_RPI)
    CFLAGS = -O2 -Wall -std=gnu99 -fgnu89-inline
else
    CFLAGS = -O2 -Wall -std=c99
endif
ifeq ($(PLATFORM),PLATFORM_WEB)
    CFLAGS = -O1 -Wall -std=c99 -s USE_GLFW=3 --shell-file ../../templates/web_shell/shell.html
    #-s ASSERTIONS=1 --preload-file resources
    #-s ALLOW_MEMORY_GROWTH=1   # to allow memory resizing
    #-s TOTAL_MEMORY=16777216   # to specify heap memory size (default = 16MB)
endif

#CFLAGSEXTRA = -Wextra -Wmissing-prototypes -Wstrict-prototypes

# define any directories containing required header files
ifeq ($(PLATFORM),PLATFORM_RPI)
    INCLUDES = -I. -I../../src -I/opt/vc/include -I/opt/vc/include/interface/vcos/pthreads
else
    INCLUDES = -I. -I../../src -I../../../src
# external libraries headers
# GLFW3
    INCLUDES += -I../../external/glfw3/include
# GLEW
    INCLUDES += -I../../external/glew/include
# OpenAL Soft
    INCLUDES += -I../../external/openal_soft/include
endif

# define library paths containing required libs
ifeq ($(PLATFORM),PLATFORM_RPI)
    LFLAGS = -L. -L../../src -L/opt/vc/lib
else
    LFLAGS = -L. -L../../src -L../../../src
    # external libraries to link with
    # GLFW3
    LFLAGS += -L../../external/glfw3/lib/$(LIBPATH)
    ifneq ($(PLATFORM_OS),OSX)
    # OpenAL Soft
        LFLAGS += -L../../external/openal_soft/lib/$(LIBPATH)
	# GLEW
        LFLAGS += -L../../external/glew/lib/$(LIBPATH)
    endif
endif

# define any libraries to link into executable
# if you want to link libraries (libname.so or libname.a), use the -lname
ifeq ($(PLATFORM),PLATFORM_DESKTOP)
    ifeq ($(PLATFORM_OS),LINUX)
        # libraries for Debian GNU/Linux desktop compiling
        # requires the following packages:
        # libglfw3-dev libopenal-dev libglew-dev libegl1-mesa-dev
        LIBS = -lraylib -lglfw3 -lGLEW -lGL -lopenal -lm -pthread -ldl
        # on XWindow could require also below libraries, just uncomment
        #LIBS += -lX11 -lXrandr -lXinerama -lXi -lXxf86vm -lXcursor
    else
    ifeq ($(PLATFORM_OS),OSX)
    	# libraries for OS X 10.9 desktop compiling
        # requires the following packages:
        # libglfw3-dev libopenal-dev libglew-dev libegl1-mesa-dev
        LIBS = -lraylib -lglfw -framework OpenGL -framework OpenAl -framework Cocoa
    else
        # libraries for Windows desktop compiling
        # NOTE: GLFW3 and OpenAL Soft libraries should be installed
        LIBS = -lraylib -lglfw3 -lglew32 -lopengl32 -lopenal32 -lgdi32
    endif
    endif
endif
ifeq ($(PLATFORM),PLATFORM_RPI)
    # libraries for Raspberry Pi compiling
    # NOTE: OpenAL Soft library should be installed (libopenal1 package)
    LIBS = -lraylib -lGLESv2 -lEGL -lpthread -lrt -lm -lbcm_host -lopenal
endif
ifeq ($(PLATFORM),PLATFORM_WEB)
    LIBS = ../../src/libraylib.bc
endif

# define additional parameters and flags for windows
ifeq ($(PLATFORM_OS),WINDOWS)
    # resources file contains windows exe icon
    # -Wl,--subsystem,windows hides the console window
    WINFLAGS = ../../src/resources -Wl,--subsystem,windows
endif

ifeq ($(PLATFORM),PLATFORM_WEB)
    EXT = .html
endif

# define all screen object files required
SCREENS = \
	screens/screen_logo.o \
	screens/screen_level00.o \
	screens/screen_level01.o \
	screens/screen_level02.o \
	screens/screen_level03.o \
	screens/screen_level04.o \
	screens/screen_level05.o \
	screens/screen_level06.o \
	screens/screen_level07.o \
	screens/screen_level08.o \
	screens/screen_level09.o \

# typing 'make' will invoke the first target entry in the file,
# in this case, the 'default' target entry is just_do
default: just_do

# compile just_do
just_do: just_do.c $(SCREENS)
	$(CC) -o $@$(EXT) $< $(SCREENS) $(CFLAGS) $(INCLUDES) $(LFLAGS) $(LIBS) -D$(PLATFORM) $(WINFLAGS)

# compile screen LOGO
screens/screen_logo.o: screens/screen_logo.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)
    
# compile screen LEVEL00
screens/screen_level00.o: screens/screen_level00.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL01
screens/screen_level01.o: screens/screen_level01.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL02
screens/screen_level02.o: screens/screen_level02.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL03
screens/screen_level03.o: screens/screen_level03.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL04
screens/screen_level04.o: screens/screen_level04.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL05
screens/screen_level05.o: screens/screen_level05.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL06
screens/screen_level06.o: screens/screen_level06.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)
    
# compile screen LEVEL07
screens/screen_level07.o: screens/screen_level07.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)
    
# compile screen LEVEL08
screens/screen_level08.o: screens/screen_level08.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# compile screen LEVEL09
screens/screen_level09.o: screens/screen_level09.c
	$(CC) -c $< -o $@ $(CFLAGS) $(INCLUDES) -D$(PLATFORM)

# clean everything
clean:
ifeq ($(PLATFORM),PLATFORM_DESKTOP)
    ifeq ($(PLATFORM_OS),OSX)
		find . -type f -perm +ugo+x -delete
		rm -f *.o
    else
    ifeq ($(PLATFORM_OS),LINUX)
		find . -type f -executable -delete
		rm -f *.o
    else
		del *.o *.exe
    endif
    endif
endif
ifeq ($(PLATFORM),PLATFORM_RPI)
	find . -type f -executable -delete
	rm -f *.o
endif
ifeq ($(PLATFORM),PLATFORM_WEB)
	del *.o *.html *.js
endif
	@echo Cleaning done

# instead of defining every module one by one, we can define a pattern
# this pattern below will automatically compile every module defined on $(OBJS)
#%.exe : %.c
#	$(CC) -o $@ $< $(CFLAGS) $(INCLUDES) $(LFLAGS) $(LIBS) -D$(PLATFORM)
