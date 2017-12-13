#include "gtest/gtest.h"

#include <iterator>

#include "intervals.h"

int distance(intervals_iterator begin, intervals_iterator end)
{
    int n = 0;
    while (begin != end)
    {
        ++begin;
        ++n;
    }
    return n;
}

TEST(Intervals, Empty) {
    auto collection = intervals();
    auto items = collection.begin();
    ASSERT_EQ(collection.end(), items);
}

TEST(Intervals, Add) {
    auto collection = intervals();
    collection.add(1, 2);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(2, first.stop_);
}


TEST(Intervals, AddContained) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.add(1, 5);
    collection.add(2, 3);
    collection.add(2, 5);
    collection.add(1, 4);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(5, first.stop_);
}

TEST(Intervals, AddExtend) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.add(2, 8);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(8, first.stop_);
}

TEST(Intervals, AddAdd) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.add(6, 8);

    ASSERT_EQ(2, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(5, first.stop_);

    ++items;
    auto second = *items;
    ASSERT_EQ(6, second.start_);
    ASSERT_EQ(8, second.stop_);
}

TEST(Intervals, AddViaRemove) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.add(6, 8);
    collection.add(2, 10);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(10, first.stop_);
}

TEST(Intervals, AddViaRemoveExtend) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.add(6, 8);
    collection.add(2, 7);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(8, first.stop_);
}

TEST(Intervals, RemoveEmpty) {
    auto collection = intervals();
    collection.remove(1, 5);

    ASSERT_EQ(0, distance(collection.begin(), collection.end()));
}

TEST(Intervals, RemoveAfter) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.remove(6, 7);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(5, first.stop_);
}

TEST(Intervals, RemoveRightAfter) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.remove(5, 6);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(5, first.stop_);
}

TEST(Intervals, RemoveCut) {
    auto collection = intervals();
    collection.add(1, 5);
    collection.remove(2, 5);

    ASSERT_EQ(1, distance(collection.begin(), collection.end()));

    auto items = collection.begin();
    auto first = *items;
    ASSERT_EQ(1, first.start_);
    ASSERT_EQ(2, first.stop_);
}
