package core;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class DBConnection {
	public static Connection create(String url, String user, String pass) {
		Connection connection = null;
		try {
			try {
				//Tim driver Access
				Class.forName("net.ucanaccess.jdbc.UcanaccessDriver");
			} catch (ClassNotFoundException e) {
				ShowAlertDialog.showErrorAlert("Error", "Class.forName Error!");
			}
			connection = DriverManager.getConnection(url, user, pass);
		} catch (Exception e) {
				ShowAlertDialog.showErrorAlert("Connect to Database","Can't connect to Database!");
		}
		return connection;
	}
	
	public static void closeConnect(Connection connection) {
		try {
			connection.close();
		} catch (SQLException e) {
			ShowAlertDialog.showErrorAlert("Error", "Can't close Connect");
		}
	}
	
	public static void closePreparedStatement(PreparedStatement pst) {
		try {
			pst.close();
		} catch (SQLException e) {
			ShowAlertDialog.showErrorAlert("Error", "Can't close PreparedStatement");
		}
	}
	
	public static void closeStatement(Statement st) {
		try {
			st.close();
		} catch (SQLException e) {
			ShowAlertDialog.showErrorAlert("Error", "Can't close Statement");
		}
	}
	
	public static void closeResultSet(ResultSet rs) {
		try {
			rs.close();
		} catch (SQLException e) {
			ShowAlertDialog.showErrorAlert("Error", "Can't close ResultSet");
		}
		
	}
}
