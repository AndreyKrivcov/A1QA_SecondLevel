package days;

import java.io.BufferedReader;
import java.io.InputStreamReader;

public class Days {

    /**
     * Start program
     * @param args Arguments (do not used) 
     */
    public static void main(String[] args) {
        
        // Create reader
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
         
        try {
            // Enter input data
            System.out.print("Enter hours : ");
            int h = Integer.parseInt(reader.readLine());
            
            System.out.print("Enter Minutes : ");
            int m = Integer.parseInt(reader.readLine());
            
            // Create main class
            var time = new TwentyFourHours(new Time(h,m));
            
            // Print required data
            time.printTime("Current time is : ");
            System.out.println("Current period is " + time.getPeriod().toString());
            
        } catch(Exception e) {
            // Print exceptions if exists
            System.out.println(e.getMessage());
        }
    }
}
