package core;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

public class ChoiceBoxData {
	private String jdbcURL;
	private String jdbcUsername;
	private String jdbcPassword; 
	//private List<String>  classList = new ArrayList<String>();
	//private List<String>  hometownList = new ArrayList<String>();
	private Connection connection;
	
	public ChoiceBoxData(String jdbcURL, String jdbcUsername, String jdbcPassword) {
		super();
		this.jdbcURL = jdbcURL;
		this.jdbcUsername = jdbcUsername;
		this.jdbcPassword = jdbcPassword;
	}
	
	public List<String> getClassList(){
		List<String> classList = new ArrayList<String>();
		Statement st = null;
		ResultSet rs = null;
		try {
			connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
			st = connection.createStatement();
			rs = st.executeQuery("SELECT classname FROM Class");
			String name;
			while(rs.next()) {
				name = rs.getString("classname");
				classList.add(name);
			}
		} catch (SQLException e) {
			e.printStackTrace();
		} finally {
			DBConnection.closeResultSet(rs);
			DBConnection.closeStatement(st);
			DBConnection.closeConnect(connection);
		}
		return classList;
	}
	
	public List<String> getHomeTownList(){
		List<String> hometownList = new ArrayList<String>();
		Statement st = null;
		ResultSet rs = null;
		try {
			connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
			st = connection.createStatement();
			rs = st.executeQuery("SELECT name FROM Hometown");
			String name;
			while(rs.next()) {
				name = rs.getString("name");
				hometownList.add(name);
			}
		} catch (SQLException e) {
			e.printStackTrace();
		} finally {
			DBConnection.closeResultSet(rs);
			DBConnection.closeStatement(st);
			DBConnection.closeConnect(connection);
		}
		return hometownList;
	}
}
