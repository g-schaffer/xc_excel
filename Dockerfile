FROM ubuntu:20.04

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update && apt-get install -y sudo wget bash cmake make git gcc g++ gfortran libboost-all-dev && \
    useradd -m docker && echo "docker:docker" | chpasswd && adduser docker sudo

RUN apt-get install -y git  cmake g++ gfortran libboost-all-dev

RUN apt-get install -y python-is-python3

RUN apt-get install -y libarpack2-dev libarpack++2-dev libcgal-dev libcgal-qt5-dev libdb-dev libf2c2-dev libglib2.0-dev libgmp3-dev libgtk2.0-dev libgtkgl2.0-dev libgtkglextmm-x11-1.2-dev libgtkglext1-dev libgtkmm-2.4-dev libgts-bin libgts-dev liblapack-dev libmumps-dev libmpfr-dev libmysql++-dev libplot-dev libsqlite3-dev libsuperlu-dev libsuitesparse-dev libvtk7-dev libx11-dev libmetis-dev

RUN apt-get install -y python3-dev cimg-dev petsc-dev tcl-dev

RUN apt-get install -y python3-vtk7 python3-numpy python3-scipy python3-sympy python3-matplotlib python3-pandas python3-sklearn python3-pip

RUN apt-get install -y gnuplot python3-pip bc graphicsmagick-imagemagick-compat

RUN apt-get clean

RUN apt-get install -y python3.9

RUN update-alternatives --install /usr/bin/python3 python3 /usr/bin/python3.9 1 && \
    update-alternatives --install /usr/bin/python python /usr/bin/python3.9 1 && \
    update-alternatives --set python3 /usr/bin/python3.9 && \
    update-alternatives --set python /usr/bin/python3.9

RUN python3.9 -m pip install mayavi
RUN python3.9 -m pip install ezdxf
RUN python3.9 -m pip install pyexcel
RUN python3.9 -m pip install pyexcel-ods
RUN python3.9 -m pip install dxfwrite   
RUN python3.9 -m pip install pycairo

COPY gmsh_install.sh gmsh_install.sh

RUN sh gmsh_install.sh

RUN mkdir build_xc && \
    cd build_xc && \
    git clone https://github.com/xcfem/xc/ xc

RUN cd build_xc && \
    mkdir build-xc && \
    cd build-xc && \
    cmake ../xc/src 

RUN cd build_xc/build-xc && \
    make -j 4

RUN cd build_xc/build-xc && \
    make install 

RUN pip install --upgrade numpy
RUN pip install --upgrade scipy
RUN pip install --upgrade matplotlib
RUN apt-get install -y python3.9-dev
RUN pip install --upgrade pycairo
RUN pip install --upgrade cycler
COPY local_install.sh build_xc/xc/python_modules/local_install_fixed.sh
RUN cd build_xc/xc/python_modules && sh local_install_fixed.sh
COPY driver.py driver.py
COPY test.py test.py
COPY test_without_warning.py test_without_warning.py
CMD ["bin/bash"]
