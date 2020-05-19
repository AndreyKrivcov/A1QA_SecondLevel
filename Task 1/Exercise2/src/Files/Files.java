
package Files;
import java.util.ArrayList;
import java.io.*;

public class Files {
    private static ArrayList<String> removeLines(String fileName, int n, int m){
        File file = new File(fileName);
        ArrayList<String> ans = new ArrayList<>();
                
        try{
            var data = read(file);
            if(data.size() < n || m < n)
                return ans;
            
            int size = 1 + Math.min(m, data.size()) - n--;
            for (int i = 0; i < size; ++i) {
                String s = data.get(n);
                ans.add(s);
                data.remove(s);
            }    
            
            write(data,file);
        }catch(IOException ex){
            System.out.println(ex.getMessage());
        }
        
        return ans;
    }
    
    private static ArrayList<String> read(File file) throws IOException{
        ArrayList<String> list = new ArrayList<>();
        
        if(!file.exists())
            throw new IOException("File doesn`t exists");
        if(file.isDirectory())
            throw new IOException("File expected");
        
        BufferedReader reader=null;
        try{
            reader = new BufferedReader(new FileReader(file));
            
            String line;
            while((line = reader.readLine()) != null)
                list.add(line);            
            
        }catch(IOException ex){
            throw ex;
        }finally{
            if(reader != null)
                reader.close();
        }
                
        return list;
    }
    
    private static void write(ArrayList<String> data, File file) throws IOException {
        if(file.isDirectory())
            throw new IOException("File expected");
        
        if(file.exists())
            file.delete();
        
        BufferedWriter writer = null;
        try{
            writer = new BufferedWriter(new FileWriter(file));
            
            for (String line : data) {
                writer.write(line + "\n");
            }
        }catch(IOException ex){
            throw ex;
        }finally{
            if(writer != null)
                writer.close();
        }
    }
    
    public static void main(String args[]) {
        
        BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        
        try {
            System.out.print("Enter path to file : ");
            String path = reader.readLine(); 
            System.out.print("Enter n value : ");
            int n = Integer.parseInt(reader.readLine());
            System.out.print("Enter m value : ");
            int m = Integer.parseInt(reader.readLine());
            
            var removedData = removeLines(path, n, m);
        
            removedData.forEach((string) -> {
                System.out.println(string);
            });
            
        } catch (IOException e) {
            System.out.println(e.getMessage());
        }
    }
}
