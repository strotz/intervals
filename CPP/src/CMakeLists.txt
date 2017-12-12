set(sources
        main.cpp)

add_executable(intervals ${sources})

install(TARGETS intervals DESTINATION bin)