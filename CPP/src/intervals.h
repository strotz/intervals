#ifndef INTERVALS_INTERVALS_H
#define INTERVALS_INTERVALS_H

#include <set>
#include <unordered_map>
#include <iterator>
#include "container.h"

struct interval
{
    int start_;
    int stop_;

    interval(int start, int stop) : start_(start), stop_(stop)
    {
    }
};

// this is simplified iterator to walk over intervals
class intervals_iterator
{
private:

    typedef std::set<int>::iterator SortedKeys;
    typedef const std::unordered_map<int, int>& ElementRefs;

    friend class intervals;

    intervals_iterator(SortedKeys iterator, ElementRefs map) :
            iterator_(iterator),
            map_(map)
    {
    }

    SortedKeys iterator_;
    ElementRefs map_;

public:

    interval current() const
    {
        int start = *iterator_;
        int stop = map_.at(start);
        return interval(start, stop);
    }

    interval operator*() const
    {
        return current();
    }

    intervals_iterator& operator++()
    {
        ++iterator_;
        return *this;
    }

    bool operator==(const intervals_iterator& __x) const
    {
        return iterator_ == __x.iterator_;
    }

    bool operator!=(const intervals_iterator& __x) const
    {
        return iterator_ != __x.iterator_;
    }
};

// poot man replacement of std::optional
template<typename Val>
struct place
{
    place() : value_(nullptr)
    {
    }

    ~place()
    {
        if (value_ != nullptr)
        {
            delete value_;
        }
    }

    void assign(Val* value)
    {
        value_ = value;
    }

    bool is_assigned() const {
        return value_ != nullptr;
    }

    const Val& operator*() const {
        return *value_;
    }

    const Val& operator->() const {
        return *value_;
    }

private:

    Val* value_;
};

class intervals
{
public:

    void add(int start, int stop)
    {
        if (start >= stop)
        {
            throw std::invalid_argument("start should be less then stop");
        }

        if (starts_.empty())
        {
            insert(start, stop);
            return;
        }

        auto pre_start = less_or_equal(starts_, start);
        if (pre_start != starts_.end())
        {
            int pre_stop = start_to_stop_.at(*pre_start);
            if (pre_stop >= stop) // interval completely contained
            {
                return;
            }
            if (pre_stop >= start) // merge requested and head
            {
                start = *pre_start;
            }
        }

        auto last_start = less_or_equal(starts_, stop);
        if (last_start != starts_.end())
        {
            int last_stop = start_to_stop_.at(*last_start);
            if (last_stop > stop) // extend requested interval to cover last
            {
                stop = last_stop;
            }
        }

        clean_overlaps(start, stop);

        insert(start, stop);
    }

    void remove(int start, int stop) {
        if (start >= stop) {
            throw std::invalid_argument("start should be less then stop");
        }

        if (starts_.empty()) {
            return;
        }

        auto head = place<interval>();
        auto tail = place<interval>();

        auto pre_start = less_or_equal(starts_, start);
        if (pre_start != starts_.end())
        {
            int pre_stop = start_to_stop_.at(*pre_start);
            if (*pre_start < start && pre_stop > start) // need to preserve head
            {
                head.assign(new interval(*pre_start, start));
                start = *pre_start;
            }
        }

        auto last_start = less_or_equal(starts_, stop);
        if (last_start != starts_.end())
        {
            int last_stop = start_to_stop_.at(*last_start);
            if (last_stop > stop) // extend requested interval to cover last
            {
                tail.assign(new interval(stop, last_stop));
            }
        }

        clean_overlaps(start, stop);

        if (head.is_assigned())
        {
            insert((*head).start_, (*head).stop_);
        }

        if (tail.is_assigned())
        {
            insert((*tail).start_, (*tail).stop_);
        }
    }

    intervals_iterator begin() const
    {
        return intervals_iterator(starts_.cbegin(), start_to_stop_);
    }

    intervals_iterator end() const
    {
        return intervals_iterator(starts_.end(), start_to_stop_);
    }

private:

    void insert(int start, int stop)
    {
        starts_.insert(start);
        start_to_stop_[start] = stop;
    }

    void clean_overlaps(int start, int stop)
    {
        std::set<int>::iterator begin = starts_.lower_bound(start);
        for (auto it = begin; it != starts_.end() && *it <= stop; ++it)
        {
            start_to_stop_.erase(*it);
            starts_.erase(it);
        }
    }


private:

    std::set<int> starts_; // sorted by start_
    std::unordered_map<int, int> start_to_stop_; // constant time for access
};

#endif //INTERVALS_INTERVALS_H
