package core;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

public class ACCM {
	private String jdbcURL;
	private String jdbcUsername;
	private String jdbcPassword; 	
	List<AdminAccount> accList= new ArrayList<AdminAccount>();
	private Connection connection;
	
	
	public ACCM(String jdbcURL, String jdbcUsername, String jdbcPassword) {
		this.jdbcURL = jdbcURL;
		this.jdbcUsername = jdbcUsername;
		this.jdbcPassword = jdbcPassword;
		accList = this.getAccountList();
	}
	
	
	public ACCM() {
		// TODO Auto-generated constructor stub
	}


	public List<AdminAccount> getAccountList(){
		List<AdminAccount> accountList = new ArrayList<AdminAccount>();
		Statement st = null;
		ResultSet rs = null;
		try {
			connection = DBConnection.create(jdbcURL, jdbcUsername, jdbcPassword);
			st = connection.createStatement();
			rs = st.executeQuery("SELECT * FROM UserAccount");
			String username;
			String password;
			while(rs.next()) {
				username = rs.getString("username");
				password = rs.getString("password");
				accountList.add(new AdminAccount(username, password));
			}
		} catch (SQLException e) {
			e.printStackTrace();
		} finally {
			DBConnection.closeResultSet(rs);
			DBConnection.closeStatement(st);
			DBConnection.closeConnect(connection);
		}
		return accountList;
	}
	
	public boolean checkAccount(AdminAccount acc) {
		for(AdminAccount compared_acc : accList) {
			if(compared_acc.equals(acc)) {
				return true;
			}
		}
		return false;
	}
}
