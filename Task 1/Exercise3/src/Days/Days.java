package Days;
public class Days {

    public static void main(String[] args) {
        try {
            var morning = new TwentyFourHours(new Time(8,05));
            var day = new TwentyFourHours(new Time(11,00));
            var evening = new TwentyFourHours(new Time(17,45));
            var night0 = new TwentyFourHours(new Time(3, 00));
            var night1 = new TwentyFourHours(new Time(21, 59));
            var night2 = new TwentyFourHours(new Time(23, 59));
            
            System.out.println(morning.getPeriod().toString());
            morning.printTime();
            System.out.println(day.getPeriod().toString());
            day.printTime();
            System.out.println(evening.getPeriod().toString());
            evening.printTime();
            
            System.out.println(night0.getPeriod().toString());
            night0.printTime();
            System.out.println(night1.getPeriod().toString());
            night1.printTime();
            System.out.println(night2.getPeriod().toString());
            night2.printTime();
        } catch(Exception e) {
            System.out.println(e.getMessage());
        }
    }
    
    public static void CompareTest(Time t1, Time t2) {
        switch(t1.compare(t2)) {
            case 0 : 
                System.out.println("Equals | " + t1.toString() + " == " + t2.toString()); 
                break;
            case 1 : 
                System.out.println("Grater | " + t1.toString() + " > " + t2.toString()); 
                break;
            case -1 : 
                System.out.println("Less | " + t1.toString() + " < " + t2.toString()); 
                break;
        }
    }
    
}
