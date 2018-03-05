TEMPLATE = app
CONFIG += console c++11
CONFIG -= app_bundle
CONFIG -= qt

LIBS += -lpthread

SOURCES += main.cpp \
    rc4.cpp

HEADERS += \
    rc4.h \
    ringbuffer.h
