#include <algorithm>
#include <cmath>
#include <cassert>
#include <cstdio>
#include "utils.hpp"

#define PI 3.14159265359
// floating point error tolerance. Set to high value for now 
// since we don't need too much precision
#define FLOAT_TOL 1e-4 

#define CONV_FACTOR_LNG 8.627
#define DEGREES_METER_LNG 0.0001
#define CONV_FACTOR_LAT 111319.9

// RP::point::point() : x(0), y(0) {}

// RP::point::point(float x, float y) : x(x), y(y)
// {
// }
// RP::point::point(const point& other)
// {
//     x = other.x;
//     y = other.y;
// }

bool RP::point::operator==(const point &p) const
{
    return x == p.x && y == p.y;
}
bool RP::point::operator!=(const point &p) const
{
    return x != p.x || y != p.y;
}

// RP::point& RP::point::operator=(const point& other)
// {
//     x = other.x;
//     y = other.y;
//     return *this;
// }

RP::line::line(point inp, point inq)
{
    p = inp;
    q = inq;
}
RP::line::line(float px, float py, float qx, float qy)
{
    p.x = px;
    p.y = py;
    q.x = qx;
    q.y = qy;
}

bool RP::polarPoint::operator==(const polarPoint &p) const
{
    if (r == p.r)
        return normalize_angle(th) == normalize_angle(p.th);
    else if (r == -p.r)
        return normalize_angle(th) == -normalize_angle(p.th);
    else
        return false;
}
bool RP::polarPoint::operator!=(const polarPoint &p) const
{
    return !(*this == p);
}

float RP::deg_to_rad(float deg)
{
    return (deg * PI / 180.0f);
}

float RP::rad_to_deg(float rad)
{
    return (rad * 180.0f / PI);
}

float RP::normalize_angle(float rad)
{
    rad = fmod(rad, 2 * PI);
    if (rad < 0)
        rad += 2 * PI;
    return rad;
}

float RP::normalize_angle_deg(float deg)
{
    deg = fmod(deg, 360);
    if (deg < 0)
        deg += 360;
    return deg;
}

RP::point RP::intersection(point A, point B, point C, point D)
{
    // Line AB represented as a1x + b1y = c1
    float a1 = B.y - A.y;
    float b1 = A.x - B.x;
    float c1 = a1 * (A.x) + b1 * (A.y);

    // Line CD represented as a2x + b2y = c2
    float a2 = D.y - C.y;
    float b2 = C.x - D.x;
    float c2 = a2 * (C.x) + b2 * (C.y);

    float determinant = a1 * b2 - a2 * b1;

    // The lines are parallel. This is simplified
    // by returning a pair of FLT_MAX
    if (-1e-7 <= determinant && determinant <= 1e-7)
        return (point{INFINITY, INFINITY});

    float x = (b2 * c1 - b1 * c2) / determinant;
    float y = (a1 * c2 - a2 * c1) / determinant;
    return (point{x, y});
}

//start, end, circle, and R are in lat/long coordinates
bool RP::segment_intersects_circle(point start,
                                   point end,
                                   point circle,
                                   float R)
{

    point direction{end.x - start.x, end.y - start.y};
    point center_to_start{start.x - circle.x, start.y - circle.y};
    float a = direction.x * direction.x + direction.y * direction.y;
    float b = 2 * (center_to_start.x * direction.x + center_to_start.y * direction.y);
    float c = (center_to_start.x * center_to_start.x + center_to_start.y * center_to_start.y) + R * R;

    float discriminant = b * b - 4 * a * c;
    if (discriminant < 0)
    {
        return (false);
    }

    discriminant = sqrt(discriminant);
    float t1 = (-b + discriminant) / (2 * a);
    float t2 = (-b - discriminant) / (2 * a);

    return ((0 <= t1 && t1 <= 1.0f) || (0 <= t2 && t2 <= 1.0f));
}

//Returns 0 if p, q, and r are colinear.
//Returns 1 if pq, qr, and rp are clockwise
//Returns 2 if pq, qr, and rp are counterclockwise
int RP::orientation(point p, point q, point r)
{
    float v = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);

    if (-1e-4 <= v && v <= 1e-4)
        return COLINEAR;

    return ((v > 0.0f) ? CLOCKWISE : COUNTERCLOCKWISE);
}

//Tells if r is on segment pq
bool RP::on_segment(point p, point q, point r)
{
    return (q.x <= std::max(p.x, r.x) && q.x >= std::min(p.x, r.x) &&
            q.y <= std::max(p.y, r.y) && q.y >= std::min(p.y, r.y));
}

//Probably returns whether p1p2 and q1q2 intersect
bool RP::segments_intersect(point p1, point p2, point q1, point q2)
{
    int o1 = orientation(p1, p2, q1);
    int o2 = orientation(p1, p2, q2);
    int o3 = orientation(q1, q2, p1);
    int o4 = orientation(q1, q2, p2);

    if (o1 != o2 && o3 != o4)
        return (true);

    if (o1 == 0 && on_segment(p1, q1, p2))
        return (true);

    if (o2 == 0 && on_segment(p1, q2, p2))
        return (true);

    if (o3 == 0 && on_segment(q1, p1, q2))
        return (true);

    if (o4 == 0 && on_segment(q1, p2, q2))
        return (true);

    return (false);
}

// returns the intersection between ab and cd (I know it's confusing since
// it's ordered differently from segments_intersect() and I won't attempt to
// defend it). returned point is at {inf, inf} if no intersection exists
RP::point RP::segments_intersection(point a, point b, point c, point d)
{
    point x = intersection(a, b, c, d);
    if (x.x == INFINITY)
        return x;
    if (!within_segment(a, b, x) || !within_segment(c, d, x))
        return point{INFINITY, INFINITY};
    return x;
}

bool RP::seg_intersects_width(point p1, point p2, point q1, point q2, float p_width, point &inters_out)
{
    line p_line = line{p1, p2};
    line left = get_moved_line(p_line, p_width / 2, true);
    line right = get_moved_line(p_line, p_width / 2, false);
    point ccw_points[]{left.p, left.q, right.q, right.p};
    line sides[4];
    for (int i = 0; i < 4; i++)
        sides[i] = line{ccw_points[i], ccw_points[(i + 1) % 4]};
    return seg_intersects_rect(line{q1, q2}, sides, inters_out);
}

bool RP::seg_intersects_rect(line seg, line sides[4], point &inters_out)
{
    point intersects[4];
    int inter_i = 0;
    for (int i = 0; i < 4; i++)
    {
        const line &side = sides[i];
        if (segments_intersect(side.p, side.q, seg.p, seg.q))
        {
            assert(inter_i <= 4);
            intersects[inter_i++] = segments_intersection(side.p, side.q, seg.p, seg.q);
        }
    }

    if (inter_i == 0)
    {
        bool within = true;
        // check if both points are inside p_line rectangle
        for (int i = 0; i < 4; i++)
        {
            const line &side = sides[i];
            const point dir{seg.p.x - side.p.x, seg.p.y - side.p.y};
            if (dot(get_ortho(side, true), dir) < 0)
            {
                within = false;
                break;
            }
        }
        if (within)
        {
            intersects[0] = seg.p;
            intersects[1] = seg.q;
            inter_i = 2;
        }
    }

    if (inter_i != 0)
    {
        if (inter_i == 1)
            inters_out = intersects[0];
        else
        {
            const point &int1 = intersects[0];
            const point &int2 = intersects[1];
            inters_out = point{(int1.x + int2.x) / 2, (int1.y + int2.y) / 2};
        }
        return true;
    }
    return false;
}

inline bool rect_int_rect_internal(RP::line r1[4], RP::line r2[4])
{
    for (unsigned int i = 0; i < 4; i++)
    {
        // get normal pointing outwards
        RP::point normal = get_ortho(r1[i], false);
        //iterate over vertices
        float minD = 1e7;
        for (unsigned int j = 0; j < 4; j++)
        {
            // take dot
            float d = (normal.x * (r2[j].p.x - r1[i].p.x) + normal.y * (r2[j].p.y - r1[i].p.y));
            if (d < minD)
                minD = d;
        }
        if (minD > 0)
            return false;
    }
    // no sep axis found
    return true;
}

bool RP::rect_intersects_rect(line r1[4], line r2[4])
{
    return rect_int_rect_internal(r1, r2) && rect_int_rect_internal(r2, r1);
}

float RP::dot(const point &u, const point &v)
{
    return u.x * v.x + u.y * v.y;
}

void RP::move_line_toward_point(RP::line &side_points, RP::point pt, float d)
{
    int orient = RP::orientation(side_points.p, side_points.q, pt);
    if (orient == 0)
        return;
    point dir = get_ortho(side_points, orient == 2);
    dir.x *= d;
    dir.y *= d;
    // printf("%f, %f\n", dir.x, dir.y);
    side_points.p.x += dir.x;
    side_points.p.y += dir.y;
    side_points.q.x += dir.x;
    side_points.q.y += dir.y;
}

RP::point RP::get_ortho(const RP::line &ln, bool ccw)
{
    point dir{ln.q.x - ln.p.x, ln.q.y - ln.p.y};
    float t = dir.x;
    if (ccw)
    {
        dir.x = -dir.y;
        dir.y = t;
    }
    else
    {
        dir.x = dir.y;
        dir.y = -t;
    }

    float norm = sqrt(dir.x * dir.x + dir.y * dir.y);
    dir.x *= 1 / norm;
    dir.y *= 1 / norm;
    return dir;
}

RP::line RP::add_length_to_line_segment(point p, point q, float length)
{
    std::pair<float, float> pq = std::make_pair(q.x - p.x, q.y - p.y); //vector
    float pq_len = sqrt(pq.first * pq.first + pq.second * pq.second);
    pq.first = length * pq.first / pq_len;
    pq.second = length * pq.second / pq_len;

    point p1 = point{q.x + pq.first, q.y + pq.second};
    point p2 = point{p.x - pq.first, p.y - pq.second};
    return line{p1, p2};
}

RP::line RP::get_moved_line(const RP::line &ln, float d, bool ccw)
{
    point dir = get_ortho(ln, ccw);
    dir.x *= d;
    dir.y *= d;

    line ret = ln;
    ret.p.x += dir.x;
    ret.p.y += dir.y;
    ret.q.x += dir.x;
    ret.q.y += dir.y;
    return ret;
}

//returns whether c is on ab assuming that abc is a line
bool RP::within_segment(point a, point b, point c)
{
    // dot product of ab and ac
    float dotprod = ((b.y - a.y) * (c.y - a.y) + (b.x - a.x) * (c.x - a.x));
    return dotprod >= -FLOAT_TOL && dotprod <= dist_sq(a, b) + FLOAT_TOL;
}

//Returns a point in the center of segment pq and then moves it R towards cur
RP::point RP::center_point_with_radius(RP::point cur, RP::point p, RP::point q, float R)
{
    point vec{-p.y + q.y, p.x - q.x};
    float len = sqrt((vec.x * vec.x) + (vec.y * vec.y));
    vec.x = vec.x * R / len;
    vec.y = vec.y * R / len;
    point result = point{(p.x + q.x) / 2.0f, (p.y + q.y) / 2.0f};
    int o = orientation(cur, p, q);
    if (o == CLOCKWISE)
    {
        result.x += vec.x;
        result.y += vec.y;
    }
    else //o is COUNTERCLOCKWISE
    {
        result.x -= vec.x;
        result.y -= vec.y;
    }
    return (result);
}

float RP::dist_sq(point p1, point p2)
{
    return ((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
}

bool RP::within_radius(point p1, point p2, float R)
{
    return (dist_sq(p1, p2) <= R * R);
}

#define R_EARTH 6371.0088 // in km

// given lat, long, bearing (in degrees), and distance (in km), returns a new point
RP::point RP::lat_long_offset(float lat1, float lon1, float brng, float dist)
{
    dist /= 1000.0f;

    lat1 = deg_to_rad(lat1);
    lon1 = deg_to_rad(lon1);
    brng = deg_to_rad(brng);
    float lat2 = asin(sin(lat1) * cos(dist / R_EARTH) + cos(lat1) * sin(dist / R_EARTH) * cos(brng));
    float lon2 = lon1 + atan2(sin(brng) * sin(dist / R_EARTH) * cos(lat1), cos(dist / R_EARTH) - sin(lat1) * sin(lat2));
    lat2 = rad_to_deg(lat2);
    lon2 = rad_to_deg(lon2);
    return (point{lat2, lon2});
}

// Given two xy coordinates in degrees, returns the distance between them in meters.
RP::point RP::lat_long_to_meters(RP::point pt, RP::point origin)
{
    return point{(pt.x - origin.x) * 87029, (pt.y - origin.y) * 111111};
}

// generates 100 points in spiral formation around origin and returns in vector
std::vector<RP::point> RP::generate_spiral()
{
    int scaleFactor = 10;
    std::vector<point> spiralPoints;

    for (int i = 0; i < 100; ++i)
    {
        int x = round(scaleFactor * i * cos(i + (PI)));
        int y = round(scaleFactor * i * sin(i + (PI)));
        spiralPoints.push_back(point{(float)x, (float)y});
#if 0
		std::cout << i << ": (" << px << ", " << py << ")" << '\n';
#endif
    }

    return spiralPoints;
}

// All angles should already be normalized and in radians
bool RP::within_angle(float ang, float lower, float upper)
{
    if (upper < lower)
    {
        return ang >= lower || ang <= upper;
    }
    // printf("Angle: %.2f; Lower: %.2f; Upper: %.2f\n", ang, lower, upper);
    return ang >= lower && ang <= upper;
}

// return if ab and ac are in the same direction, assuming abc is a line
bool RP::same_dir(point a, point b, point c)
{
    return ((b.x - a.x) * (c.x - a.x) + (b.y - a.y) * (c.y - a.y)) >= -FLOAT_TOL;
}

RP::point RP::polar_to_cartesian(point origin, float r, float theta)
{
    return RP::point{origin.x + r * std::cos(theta), origin.y + r * std::sin(theta)};
}

float RP::relative_angle(point o, point p)
{
    return std::atan((p.y - o.y) / (p.x - o.x));
}

bool RP::same_point(const point &p, const point &q, float tol)
{
    return fabs(p.x - q.x) + fabs(p.y - q.y) <= tol;
}

bool RP::closeEnough(float a, float b, float tol)
{
    return fabs(a - b) <= tol;
}

bool RP::angleCloseEnough(float deg1, float deg2, float degtol)
{
    return closeEnough(normalize_angle_deg(deg1), normalize_angle_deg(deg2), degtol);
}

RP::point RP::convertToLatLng(float lat, float lng, float dir, float dist, float angle) {
		float delta_x = dist * cos(angle + dir + M_PI/2);
		float delta_y = dist * sin(angle + dir + M_PI/2);
		//std::cout << "delta_x: " << delta_x << " delta_y: " << delta_y << "\n";
		float delta_lng = delta_x / CONV_FACTOR_LNG * DEGREES_METER_LNG;
		float delta_lat = delta_y / CONV_FACTOR_LAT;
		//std::cout << "delta_lat: " << delta_lat << " delta_lng: " << delta_lng << "\n";
		point p;
		p.x = delta_lat + lat;
		p.y = delta_lng + lng;
		return p;
}