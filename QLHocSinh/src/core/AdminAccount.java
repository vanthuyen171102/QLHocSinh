package core;

public class AdminAccount {
	private String username;
	private String password;
	
	public AdminAccount(String username, String password) {
		this.username = username;
		this.password = password;
	}
	
	public String getUsername() {
		return username;
	}
	public void setUsername(String username) {
		this.username = username;
	}
	public String getPassword() {
		return password;
	}
	public void setPassword(String password) {
		this.password = password;
	}
	@Override
	public boolean equals(Object obj) {
		AdminAccount acc = (AdminAccount)obj;
		if(acc.getUsername().equals(this.getUsername()) && acc.getPassword().equals(this.getPassword()))
				return true;
		return false;
	}
}
