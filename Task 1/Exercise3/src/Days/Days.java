package Days;

import java.io.BufferedReader;
import java.io.InputStreamReader;

public class Days {

    public static void main(String[] args) {
        
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
         
        try {
            System.out.print("Enter hours : ");
            int h = Integer.parseInt(reader.readLine());
            
            System.out.print("Enter Minutes : ");
            int m = Integer.parseInt(reader.readLine());
            
            var time = new TwentyFourHours(new Time(h,m));
            
            time.printTime("Current time is : ");
            System.out.println("Current period is " + time.getPeriod().toString());
            
        } catch(Exception e) {
            System.out.println(e.getMessage());
        }
    }
}
