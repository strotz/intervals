set(CMAKE_CXX_STANDARD 17)

set(test_sources
        main.cpp)

add_executable(testIntervals ${test_sources})

include_directories(../../src)

target_link_libraries(testIntervals
        libgtest
        libgmock
        )

add_test(NAME testIntervals
        COMMAND testIntervals)

install(TARGETS testIntervals DESTINATION bin)