package gui;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;

import core.DBConnection;
import core.ShowAlertDialog;
import core.Student;

public class StudentManagement {
	private String jdbcURL;
	private String jdbcUsername;
	private String jdbcPassword; 	
	
	private Connection connection;
	
	public StudentManagement(String jdbcURL, String jdbcUsername, String jdbcPassword) {
		this.jdbcURL = jdbcURL;
		this.jdbcUsername = jdbcUsername;
		this.jdbcPassword = jdbcPassword;
	}
	
	private static java.sql.Date convertStringToSqlDate(String sDate) throws ParseException{
		java.util.Date uDate = new SimpleDateFormat("dd/MM/yyyy").parse(sDate);
		java.sql.Date sqlDate = new java.sql.Date(uDate.getTime());
        return sqlDate;
    }
	
	public boolean addStudent(Student std) throws ParseException {
		/// Tạo kết nối database với 3 tham số truyền vào
		connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
		// Tạo câu truy vấn kiểu prepare
		String sqlCommand = "INSERT INTO Student(code,name,sex,class,birthday,hometown,phone,email,url_photo) "
				            + "VALUES(?,?,?,?,?,?,?,?,?)";

		PreparedStatement pst =null;
		
		boolean result = false;
		try {
			// Tạo đối tượng truy vấn kiểu Prepare
			pst = connection.prepareStatement(sqlCommand);
			// Đưa dữ liệu vào các vị trí dấu hỏi (?)
			pst.setString(1, std.getCode());
			pst.setString(2, std.getFullname());
			pst.setString(3, std.getSex());
			pst.setString(4, std.getClass_());
			pst.setDate(5, convertStringToSqlDate(std.getBirthday()));
			pst.setString(6, std.getHometown());
			pst.setString(7, std.getPhone());
			pst.setString(8, std.getEmail());
			pst.setString(9, std.getPhoto());
			// Thực hiện chạy câu truy vấn
			int i = pst.executeUpdate();
			// Nếu thành công, 1 bản ghi được thêm vào
			if (i == 1) {
				result = true;
			}
		} catch (SQLException e) {
		} finally { // Đóng kết nối trong khối finally (bắt buộc chạy)
			DBConnection.closePreparedStatement(pst);
			DBConnection.closeConnect(connection);
		}
		return result;
	}
	
	public boolean deleteStudent(String studentCode) {
		connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
		String sqlCommand  = "DELETE FROM Student WHERE code=?";
		PreparedStatement pst = null;
		
		boolean result = false;
		try {
			pst = connection.prepareStatement(sqlCommand);
			pst.setString(1, studentCode);
			int i = pst.executeUpdate();
			if(i == 1) {
				result  = true;
			}
		} catch (SQLException e) {
		} finally {
			DBConnection.closePreparedStatement(pst);
			DBConnection.closeConnect(connection);
		}
		return result;
	}
	
	public boolean updateStudent(Student std) throws ParseException {
		connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
		String sqlCommand = "UPDATE Student SET name=?,sex=?,class=?,birthday=?,hometown=?,phone=?,email=?,url_photo=?"+"WHERE code=?";
		PreparedStatement pst = null;
		boolean result  = false;
		try {
			pst = connection.prepareStatement(sqlCommand);
			pst.setString(1, std.getFullname());
			pst.setString(2, std.getSex());
			pst.setString(3, std.getClass_());
			pst.setDate(4, convertStringToSqlDate(std.getBirthday()));
			pst.setString(5, std.getHometown());
			pst.setString(6, std.getPhone());
			pst.setString(7, std.getEmail());
			pst.setString(8, std.getPhoto());
			pst.setString(9, std.getCode());
			int i = pst.executeUpdate();
			if(i==1) {
				result  = true;
			}
		} catch (SQLException e)  {
		} finally {
			DBConnection.closePreparedStatement(pst);
			DBConnection.closeConnect(connection);
		}
		return result;
	}
	
	public List<Student> getStudentList(){
		List<Student> studentList = new ArrayList<Student>();
		Statement st = null;
		ResultSet rs = null;
		try {
			connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
			st = connection.createStatement();
			rs = st.executeQuery("SELECT code,name,sex,class,birthday,hometown,phone,email,url_photo FROM Student");
			String code;
			String fullname;
			String sex;
			String class_;
			String birthday;
			String hometown;
			String phone;
			String email;
			String url;
			while(rs.next()) {
				code = rs.getString(1);
				fullname = rs.getString(2);
				sex = rs.getString(3);
				class_ = rs.getString(4);
				DateFormat df = new SimpleDateFormat("dd/MM/yyyy");
				birthday = df.format(rs.getDate(5));
				hometown = rs.getString(6);
				phone = rs.getString(7);
				email = rs.getString(8);
				url = rs.getString(9);
				studentList.add(new Student(code, fullname, sex, class_, birthday, hometown, phone, email, url));
			}
		} catch (SQLException e) {
			ShowAlertDialog.showErrorAlert("Error","Can't read date from Database!");
		} finally {
			DBConnection.closeResultSet(rs);
			DBConnection.closeStatement(st);
			DBConnection.closeConnect(connection);
		}
		return studentList;
	}
	
	
}
