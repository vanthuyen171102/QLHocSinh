package gui;


import java.io.File;
import java.net.URL;
import java.util.List;
import java.util.ResourceBundle;

import core.ShowAlertDialog;
import core.Student;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.collections.transformation.FilteredList;
import javafx.collections.transformation.SortedList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.fxml.Initializable;
import javafx.scene.Node;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.scene.control.TextField;
import javafx.scene.control.cell.PropertyValueFactory;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.MouseEvent;
import javafx.stage.Stage;
import javafx.stage.StageStyle;

public  class  HomeController implements Initializable{
	private double xOffset = 0;
	private double yOffset = 0;

	
	@FXML
	private TableView<Student> table = new TableView<Student>();
	@FXML
	private TableColumn<Student,String> codeCol;
	@FXML
	private TableColumn<Student,String> nameCol;
	@FXML
	private TableColumn<Student,String> sexCol;
	@FXML
	private TableColumn<Student,String> classCol;
	@FXML
	private TableColumn<Student,String> birthdayCol;
	@FXML
	private TableColumn<Student,String> hometownCol;
	@FXML
	private TableColumn<Student,String> phoneCol;
	@FXML
	private TableColumn<Student,String> emailCol;
	@FXML
	ImageView photo = new ImageView();
	@FXML
	private TextField codeTF;
	@FXML
	private TextField nameTF;
	@FXML
	private TextField sexTF;
	@FXML
	private TextField hometownTF;
	@FXML
	private TextField classTF;
	@FXML
	private TextField phoneTF;
	@FXML
	private TextField emailTF;
	@FXML
	private TextField dateTF;
	@FXML
	private TextField keywordTF;
	StudentManagement stdm = new StudentManagement("jdbc:ucanaccess://\\JAVA\\Workspace\\652591_VuVanThuyen\\Database\\stdm.accdb","","");
	ObservableList<Student> obsStudentList;
	SortedList<Student> sortedData;
	
	
	@Override
	public void initialize(URL location, ResourceBundle resources) {
			codeCol.setCellValueFactory(new PropertyValueFactory<Student, String>("code"));
			nameCol.setCellValueFactory(new PropertyValueFactory<Student, String>("fullname"));
			sexCol.setCellValueFactory(new PropertyValueFactory<Student, String>("sex"));
			classCol.setCellValueFactory(new PropertyValueFactory<Student, String>("class_"));
			birthdayCol.setCellValueFactory(new PropertyValueFactory<Student, String>("birthday"));
			hometownCol.setCellValueFactory(new PropertyValueFactory<Student, String>("hometown"));
			phoneCol.setCellValueFactory(new PropertyValueFactory<Student, String>("phone"));
			emailCol.setCellValueFactory(new PropertyValueFactory<Student, String>("email"));
			List<Student> studentList = stdm.getStudentList();
			obsStudentList = FXCollections.observableArrayList(studentList);
			table.setItems(obsStudentList);
			
			FilteredList<Student> filteredData = new FilteredList<>(obsStudentList, b->true);
			keywordTF.textProperty().addListener((observable,oldValue,newValue) -> {
				filteredData.setPredicate(student ->{
						if(newValue.isEmpty() || newValue == null ) {
							return true;
						}
						String searchKeyword = newValue.toLowerCase();
						if(student.getCode().toLowerCase().indexOf(searchKeyword)==0) {
							return true;
						}
						return false;
				});
			});
			sortedData = new SortedList<>(filteredData);
			sortedData.comparatorProperty().bind(table.comparatorProperty());
			table.setItems(sortedData);
	}

	
	@FXML
	public void onClickClose() {
		System.exit(0);
	}
	@FXML
	public void onClickMinimize(ActionEvent event) {
		Stage stage = (Stage)((Node)(event.getSource())).getScene().getWindow();
		stage.setIconified(true);
	}
	@FXML
	public void onMousePressed(MouseEvent event) {
		xOffset=event.getSceneX();
		yOffset=event.getSceneY();
	}
	@FXML
	public void onMouseDragged(MouseEvent event) {
		Node node = (Node) event.getSource();
	    Stage primaryStage = (Stage) node.getScene().getWindow();
		primaryStage.setX(event.getScreenX()-xOffset);
		primaryStage.setY(event.getScreenY()-yOffset);
	}
	@FXML
	public void onClickRow() {
			try {
			codeTF.setText(table.getSelectionModel().getSelectedItem().getCode());
			nameTF.setText(table.getSelectionModel().getSelectedItem().getFullname());
			classTF.setText(table.getSelectionModel().getSelectedItem().getClass_());
			sexTF.setText(table.getSelectionModel().getSelectedItem().getSex());
			hometownTF.setText(table.getSelectionModel().getSelectedItem().getHometown());
			phoneTF.setText(table.getSelectionModel().getSelectedItem().getPhone());
			emailTF.setText(table.getSelectionModel().getSelectedItem().getEmail());
			dateTF.setText(table.getSelectionModel().getSelectedItem().getBirthday());
			String url = table.getSelectionModel().getSelectedItem().getPhoto();
			File file = new File(url);
		    Image image = new Image(file.toURI().toString());
			photo.setImage(image);
			}catch (Exception e) {
			}
	}
	@FXML 
	public void onClickAdd(){
		try {
			table.getScene().getWindow().hide();
			Parent root = FXMLLoader.load(getClass().getResource("AddScene.fxml"));
			//Thêm layout vào khung chứa scene
			Stage stage = new Stage();
			Scene scene = new Scene(root);
			stage.initStyle(StageStyle.UNDECORATED);
			//Thêm scene vào khung chứa stage
			stage.setScene(scene);
			stage.show();
		} catch (Exception e) {
			ShowAlertDialog.showErrorAlert("Error","Cant't show AddScene");
		}
	}
	@FXML 
	public void onClickEdit(ActionEvent event) {
		int selectedIndex = table.getSelectionModel().getSelectedIndex();
		if(selectedIndex<0) {
			ShowAlertDialog.showWarningAlert("Not Selected Student","You haven't selected students yet");
		}
		else {
		try {
			((Node)(event.getSource())).getScene().getWindow().hide();
			FXMLLoader fxmlLoader = new FXMLLoader(getClass().getResource("EditScene.fxml"));
			Parent root = (Parent)fxmlLoader.load();
			File url = new File(table.getSelectionModel().getSelectedItem().getPhoto());
			EditStudentController controller = fxmlLoader.getController();
			
			controller.setCodeTF(codeTF.getText());
			controller.setUrlTF(url.getAbsolutePath());
			controller.setNameTF(nameTF.getText());
			controller.setGender(sexTF.getText());
			controller.setDatePicker(dateTF.getText());
			controller.setClass(classTF.getText());
			controller.setHometown(hometownTF.getText());
			controller.setPhoneTF(phoneTF.getText());
			controller.setEmailTF(emailTF.getText());
			
			Stage stage = new Stage();
			stage.initStyle(StageStyle.UNDECORATED);
			stage.setScene(new Scene(root));
			stage.show();
			}catch (Exception e) {
				ShowAlertDialog.showErrorAlert("Error","Cant't show Edit");
			}
		}
	}
	@FXML 
	public void onClickDelete(ActionEvent event) {
		int selectedIndex = table.getSelectionModel().getSelectedIndex();
		
		if(selectedIndex<0) {
			ShowAlertDialog.showWarningAlert("Not Selected Student","You haven't selected students yet");
		}
		else {
			if(ShowAlertDialog.showConfirmationAlert("Delete Student", "Are you sure want to delete this student?")){
				Student std = table.getItems().get(selectedIndex);
				boolean deleteResult = stdm.deleteStudent(std.getCode());
				if(deleteResult) {
					int sourceIndex = sortedData.getSourceIndexFor(obsStudentList,selectedIndex);
					obsStudentList.remove(sourceIndex);					
					ShowAlertDialog.showSuccessfulAlert("Delete Student", "Delete Successful!");
				} 
				else {
					ShowAlertDialog.showErrorAlert("Delete Student","Delete Failed!");
				}
			}
		}
	}
}
