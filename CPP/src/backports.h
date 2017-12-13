
#ifndef INTERVALS_BACKPORTS_H
#define INTERVALS_BACKPORTS_H

// poor man replacement of std::optional
template<typename Val>
struct place {
	place() : value_(nullptr) {
	}

	~place() {
		if (value_ != nullptr) {
			delete value_;
		}
	}

	void assign(Val *value) {
		value_ = value;
	}

	bool is_assigned() const {
		return value_ != nullptr;
	}

	const Val &operator*() const {
		return *value_;
	}

	const Val &operator->() const {
		return *value_;
	}

private:

	Val *value_;
};

#endif //INTERVALS_BACKPORTS_H
