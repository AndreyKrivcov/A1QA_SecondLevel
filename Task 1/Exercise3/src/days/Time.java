package days;

/**
 * Classthat represent time in format hh:mm
 * @author andrey
 */
class Time {
    /**
     * Constructor
     * @param h hours
     * @param m minutes
     */
    public Time(Hour h, Minute m){
        this.h = h;
        this.m = m;
    }
    /**
     * Constructor
     * @param h hours [0;23]
     * @param m minutes [0;23]
     * @throws Exception
     */
    public Time(int h, int m) throws Exception {
        this(new Hour(h), new Minute(m));
    }
    
    private TimeItem h;
    private TimeItem m;
     
    /**
     * Convert time to string
     * @return time in format hh:mm
     */
    @Override
    public String toString() {
        return h.toString() + ":" + (m.get() < 10 ? "0" : "") + m.toString();
    }
    
    /**
     * Conpare time
     * @param t time
     * @return (1 - if current time grater than t) 
     * (0 - if current time equals to t) 
     * (-1 if current time less than t)
     */
    public int compare(Time t) {
        int compare_h = h.compare(t.h);        
        return (compare_h == 0 ? m.compare(t.m) : compare_h);
    }
}

/**
 * Class that represent common methods for it`s child objects.
 * It was created to awoid doblicated methods and variables.
 * @author andrey
 */
abstract class TimeItem {
    protected TimeItem(int t) {
        this.t = t;
    }
    private int t;
    
    /**
     * Return time period
     * @return 
     */
    public int get() {
        return t; 
    }
    
    /**
     * Compare time period
     * @param item time (hours or minutes)
     * @return (1 - if current time grater than t) 
     * (0 - if current time equals to t) 
     * (-1 if current time less than t)
     */
    public int compare(TimeItem item) {
        if(item.t == t) {
            return 0;
        }
        
        return (t > item.t ? 1 : -1);
    }
    
    /**
     * Convert time item to string that represents by numeric time item
     * @return numeric time item converted to tring
     */
    @Override
    public String toString() {
        return Integer.toString(t);
    }
}

/**
 * Class hour
 * this class throws exceprion while it`s instance 
 * when time is out of border [0;23]
 * @author andrey
 */
class Hour extends TimeItem {
    /**
     * Constructor
     * @param h hour [0;23]
     * @throws Exception 
     */
    public Hour(int h) throws Exception {
        super(h);
        if(h < 0 || h > 23) {
            throw new Exception("Hours must be inside diaposone [0;23]");
        }
    }
}

/**
 * Class minute
 * this class throws exceprion while it`s instance 
 * when time is out of border [0;59]
 * @author andrey
 */
class Minute extends TimeItem {
    /**
     * Constrictor
     * @param m minutes [0;59]
     * @throws Exception 
     */
    public Minute(int m) throws Exception {
        super(m);
        if(m < 0 || m > 59) {
            throw new Exception("Minute must be inside diaposone [0;59]");
        }
    }
}