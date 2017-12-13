set(CMAKE_CXX_STANDARD 14)

include_directories(../../src)

set(
        test_sources
        container_tests.h
        container_tests.cpp
        main.cpp
        intervals_tests.cpp)

add_executable(testIntervals ${test_sources})

target_link_libraries(testIntervals
        libgtest
        libgmock
        )

add_test(NAME testIntervals
        COMMAND testIntervals)
