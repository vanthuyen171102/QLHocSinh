package core;

public class Student {
	private String code;
	private String fullname;
	private String sex;
	private String birthday;
	private String phone;
	private String class_;
	private String hometown;
	private String email;
	private String photo;
	public Student(String code, String fullname, String sex, String class_, String birthday, String hometown,String phone,String email,String photo) {
		this.code = code;
		this.fullname = fullname;
		this.sex = sex;
		this.birthday = birthday;
		this.phone = phone;
		this.class_ = class_;
		this.hometown = hometown;
		this.email = email;
		this.photo = photo;
	}
	
	public String getPhoto() {
		return photo;
	}

	public void setPhoto(String photo) {
		this.photo = photo;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getPhone() {
		return phone;
	}
	public void setPhone(String phone) {
		this.phone = phone;
	}
	public String getClass_() {
		return class_;
	}
	public void setClass_(String class_) {
		this.class_ = class_;
	}
	
	public String getHometown() {
		return hometown;
	}
	public void setHometown(String hometown) {
		this.hometown = hometown;
	}
	public String getCode() {
		return code;
	}
	public void setCode(String code) {
		this.code = code;
	}
	public String getFullname() {
		return fullname;
	}
	public void setFullname(String fullname) {
		this.fullname = fullname;
	}
	public String getSex() {
		return sex;
	}
	public void setSex(String sex) {
		this.sex = sex;
	}
	public String getBirthday() {
	     return birthday;
	}
	public void setBirthday(String birthday) {
		this.birthday = birthday;
	}
	
}
