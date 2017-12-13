#include <iostream>

#include "container.h"
#include "intervals.h"

int main() {
    auto collection = intervals();
    collection.add(1, 5);
    collection.remove(2, 3);
    collection.add(6, 8);
    collection.remove(4, 7);
    collection.add(2, 7);

    for(auto it = collection.begin(); it != collection.end(); ++it)
    {
        auto item = *it;
        std::cout << "["  << item.start_ << ", " << item.stop_ << "]";
    }

    return 0;
}