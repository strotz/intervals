#include "gtest/gtest.h"

#include "container_tests.h"

TEST(Container, Positive) {
	EXPECT_EQ(1,1);
}

TEST(Container, SetBounds) {
    auto starts = std::set<int>();
    starts.insert(1);
    starts.insert(4);

    auto lb1 = starts.lower_bound(1);
    EXPECT_EQ(1, *lb1);

    auto ub1 = starts.upper_bound(1);
    EXPECT_EQ(4, *ub1);

    auto lb2 = starts.lower_bound(2);
    EXPECT_EQ(4, *lb2);
}

TEST(Container, SetPredecessor) {
    auto starts = std::set<int>();
    starts.insert(1);
    starts.insert(4);

    auto p0 = less_or_equal(starts, 0);
    EXPECT_EQ(starts.end(), p0);

    auto p1 = less_or_equal(starts, 1);
    EXPECT_EQ(1, *p1);

    auto p2 = less_or_equal(starts, 2);
    EXPECT_EQ(1, *p2);

    auto p4 = less_or_equal(starts, 4);
    EXPECT_EQ(4, *p4);

    auto p5 = less_or_equal(starts, 5);
    EXPECT_EQ(4, *p5);
}