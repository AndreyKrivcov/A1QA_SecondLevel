
package files;
import java.util.ArrayList;
import java.io.*;

/**
 * Class that represent program and create main method
 * @author andrey
 */
public class Files {
    /**
     * Method that remove lines and rewrite file with old data
     * @param fileName string that represent path to the file
     * @param n number of first string that will be removed
     * @param m nubber of last string that will be removed
     * @return removed lines
     */
    private static ArrayList<String> removeLines(String fileName, int n, int m){
        
        // Create file object and returning object
        File file = new File(fileName);
        ArrayList<String> ans = new ArrayList<>();
                
        try{
            
            // Read file
            var data = read(file);
            if(data.size() < n || m < n) {
                return ans;
            }
            
            // Remove lines and save it into "ans" collection
            int size = 1 + Math.min(m, data.size()) - n--;
            for (int i = 0; i < size; ++i) {
                String s = data.get(n);
                ans.add(s);
                data.remove(s);
            }    
            
            // rewrite file without of removed lines
            write(data,file);
        }catch(IOException ex){
            System.out.println(ex.getMessage());
        }
        
        return ans;
    }
    
    /**
     * Read file data
     * @param file file object
     * @return read data
     * @throws IOException 
     */
    private static ArrayList<String> read(File file) throws IOException{
        ArrayList<String> list = new ArrayList<>();
        
        // Check file
        if(!file.exists()) {
            throw new IOException("File doesn`t exists");
        }
        if(file.isDirectory()) {
            throw new IOException("File expected");
        }
        
        BufferedReader reader=null;
        try{
            // Create reader
            reader = new BufferedReader(new FileReader(file));
            
            // Read data and save it
            String line;
            while((line = reader.readLine()) != null) {
                list.add(line);            
            }
            
        }catch(IOException ex) {
            throw ex;
        }finally {
            if(reader != null) {
                reader.close();
            }
        }
                
        return list;
    }
    
    /**
     * Rewrite data
     * @param data new file data
     * @param file file object
     * @throws IOException 
     */
    private static void write(ArrayList<String> data, File file) throws IOException {
        
        // Check file and remove old one
        if(file.isDirectory()) {
            throw new IOException("File expected");   
        }
        if(file.exists()) {
            file.delete();
        }
        
        // Create writer
        BufferedWriter writer = null;
        try{
            writer = new BufferedWriter(new FileWriter(file));
            
            // Write data line by line
            for (String line : data) {
                writer.write(line + "\n");
            }
        }catch(IOException ex){
            throw ex;
        }finally{
            if(writer != null) {
                writer.close();
            }
        }
    }
    
    /**
     * Main methods
     * @param args arguments (do not used)
     */
    public static void main(String args[]) {
        
        // Create cnsole reader
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        
        try {
            // Read input data
            System.out.print("Enter path to file : ");
            String path = reader.readLine(); 
            System.out.print("Enter n value : ");
            int n = Integer.parseInt(reader.readLine());
            System.out.print("Enter m value : ");
            int m = Integer.parseInt(reader.readLine());
            
            // Call method that remove data and write file with rest data
            var removedData = removeLines(path, n, m);
        
            // Print removed data to the console
            removedData.forEach((string) -> {
                System.out.println(string);
            });
            
        } catch (IOException e) {
            System.out.println(e.getMessage());
        }
    }
}
