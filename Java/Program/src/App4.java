public class App4 {
    public static void main(String[] args) {
        String weeks[] = {"월요일", "화요일",  "수요일",  "목요일",  "금요일",  "토요일",  "일요일",};
        int cnt = 1;
        for (int i = 1; i < 7; i++){
            if (i%3 == 0)
            break;
            cnt++;
            
        }
        System.out.print("오늘은" + weeks[cnt]);
    }
}

