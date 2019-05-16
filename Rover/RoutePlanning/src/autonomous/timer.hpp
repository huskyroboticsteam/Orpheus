#ifndef NG2_TIMER_HPP
#define NG2_TIMER_HPP

#include <chrono>
#include <cstdio>

namespace RP
{
class Timer
{
  public:
    Timer() : beg_(clock_::now()) {}
    void reset() { beg_ = clock_::now(); }
    // return elapsed time in seconds
    double elapsed() const
    {
        return std::chrono::duration_cast<second_>(clock_::now() - beg_).count();
    } 

  private:
    typedef std::chrono::high_resolution_clock clock_;
    typedef std::chrono::duration<double, std::ratio<1>> second_;
    std::chrono::time_point<clock_> beg_;
};
} // namespace RP

#endif