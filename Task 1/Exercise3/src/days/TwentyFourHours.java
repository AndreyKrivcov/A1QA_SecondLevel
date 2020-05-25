package days;

/**
 * Class that represent 24 hours daily period
 * @author andrey
 */
class TwentyFourHours {
    /**
     * Constructor
     * @param t Setted time
     */
    public TwentyFourHours(Time t) {
        this.t = t;
    }
    
    private Time t;
    
    /**
     * Print time period without of any messages
     */
    public void printTime() {
        printTime("");
    }
    
    /**
     * Print time period with message
     * @param message Message
     */
    public void printTime(String message) {
        System.out.println(message + t.toString());
    }
    
    /**
     * Get current period (Morning/Day/Evening/Night)
     * @return period of the day
     * @throws Exception
     */
    public PeriodsOfTheDay getPeriod() throws Exception {
        return getPeriod(t);
    }
    
    /**
     * Class that keeps period border and its name. 
     * This class is using only inside main class
     */
    private static class PeriodsItem {
        public PeriodsItem(Time up, Time low, PeriodsOfTheDay period) {
            UP_BORDER = up;
            LOW_BORDER = low;
            this.PERIOD = period;
        }
        public final Time UP_BORDER;
        public final Time LOW_BORDER;
        public final PeriodsOfTheDay PERIOD;
    }
            
    static PeriodsItem[] periods = null ;
    
    /**
     * Return period for the given time
     * @param t Time
     * @return period of the day
     * @throws Exception
     */
    public static PeriodsOfTheDay getPeriod (Time t) throws Exception {
                
        // Set period borders if this is first call
        if(periods == null) {
            periods = new PeriodsItem[]{
                new PeriodsItem(new Time(6,0), new Time(11,0), PeriodsOfTheDay.Morning),
                new PeriodsItem(new Time(11,0), new Time(17,0), PeriodsOfTheDay.Day),
                new PeriodsItem(new Time(17,0), new Time(22,0), PeriodsOfTheDay.Evening),
                new PeriodsItem(new Time(22,0), new Time(6,0), PeriodsOfTheDay.Night)
            };
        }
        
        // Check period and return answer
        for (PeriodsItem period : periods) {
            
            boolean up = t.compare(period.UP_BORDER) >= 0;
            boolean down = t.compare(period.LOW_BORDER) == -1;
            
            if((period.PERIOD == PeriodsOfTheDay.Night ? up || down : up && down)) {
                return period.PERIOD;
            }
        }
        
        return null;
    }
}

/**
 * Period types
 * @author andrey
 */
enum PeriodsOfTheDay {
    Morning,
    Day,
    Evening,
    Night
}
