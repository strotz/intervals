#ifndef INTERVALS_CONTAINER_H
#define INTERVALS_CONTAINER_H

template<typename Set>
typename Set::const_iterator less_or_equal(Set const& m, typename Set::key_type const& k) {
    typename Set::const_iterator it = m.upper_bound(k);
    if(it != m.begin()) {
        return --it;
    }
    return m.end();
}

template<typename Set>
typename Set::iterator less_or_equal(Set & m, typename Set::key_type const& k) {
    typename Set::iterator it = m.upper_bound(k);
    if(it != m.begin()) {
        return --it;
    }
    return m.end();
}

#endif