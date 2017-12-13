#include <iostream>

#include "intervals.h"

void dump(intervals &collection) {
	for (auto item : collection) {
		std::cout << "[" << item.start_ << ", " << item.stop_ << "]";
	}
	std::cout << std::endl;
}

int main() {
    auto collection = intervals();
    collection.add(1, 5);
	dump(collection);

    collection.remove(2, 3);
	dump(collection);

    collection.add(6, 8);
	dump(collection);

    collection.remove(4, 7);
	dump(collection);

    collection.add(2, 7);
	dump(collection);


    return 0;
}