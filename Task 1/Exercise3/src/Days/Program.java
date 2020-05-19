/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Days;

/**
 *
 * @author andrey
 */

abstract class TimeItem {
    protected TimeItem(int t) {this.t = t;}
    private int t;
    
    public int get() {return t; }
    public int compare(TimeItem item) {
        if(item.t == t)
            return 0;
        return (t > item.t ? 1 : -1);
    }
    
    @Override
    public String toString() {
        return Integer.toString(t);
    }
}

class Hour extends TimeItem {
    public Hour(int h) throws Exception {
        super(h);
        if(h < 0 || h > 23)
            throw new Exception("Hours must be inside diaposone [0;23]");
    }
}

class Minute extends TimeItem {
    public Minute(int m) throws Exception {
        super(m);
        if(m < 0 || m > 59)
            throw new Exception("Minute must be inside diaposone [0;23]");
    }
}

class Time {
    public Time(Hour h, Minute m){
        this.h = h;
        this.m = m;
    }
    public Time(int h, int m) throws Exception {
        this(new Hour(h), new Minute(m));
    }
    
    private TimeItem h, m;
     
    @Override
    public String toString()
    {
        return h.toString() + ":" + (m.get() < 10 ? "0" : "") + m.toString();
    }
    
    public int compare(Time t) {
        int compare_h = h.compare(t.h);        
        return (compare_h == 0 ? m.compare(t.m) : compare_h);
    }
}

enum PeriodsOfTheDay {
    Morning,
    Day,
    Evening,
    Night
}

class TwentyFourHours {
    public TwentyFourHours(Time t) {
        this.t = t;
    }
    
    private Time t;
    
    public void printTime() {
        printTime("");
    }
    
    public void printTime(String message) {
        System.out.println(message + t.toString());
    }
    
    public PeriodsOfTheDay getPeriod() throws Exception {
        return getPeriod(t);
    }
    
    private static class PeriodsItem {
        public PeriodsItem(Time up, Time low, PeriodsOfTheDay period) {
            upBorder = up;
            lowBorder = low;
            this.period = period;
        }
        public final Time upBorder, lowBorder;
        public final PeriodsOfTheDay period;
    }
            
    static PeriodsItem[] periods = null ;
    
    public static PeriodsOfTheDay getPeriod (Time t) throws Exception {
                
        if(periods == null) {
            periods = new PeriodsItem[]{
                new PeriodsItem(new Time(6,0), new Time(11,0), PeriodsOfTheDay.Morning),
                new PeriodsItem(new Time(11,0), new Time(17,0), PeriodsOfTheDay.Day),
                new PeriodsItem(new Time(17,0), new Time(22,0), PeriodsOfTheDay.Evening),
                new PeriodsItem(new Time(22,0), new Time(6,0), PeriodsOfTheDay.Night)
            };
        }
        
        for (PeriodsItem period : periods) {
            
            boolean up = t.compare(period.upBorder) >= 0;
            boolean down = t.compare(period.lowBorder) == -1;
            
            if((period.period == PeriodsOfTheDay.Night ? up || down : up && down))
                return period.period;
        }
        
        return null;
    }
}
