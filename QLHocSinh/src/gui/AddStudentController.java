package gui;

import java.io.File;
import java.net.URL;
import java.text.ParseException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ResourceBundle;

import core.ChoiceBoxData;
import core.ShowAlertDialog;
import core.Student;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.fxml.Initializable;
import javafx.scene.Node;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.ChoiceBox;
import javafx.scene.control.DatePicker;
import javafx.scene.control.RadioButton;
import javafx.scene.control.TextField;
import javafx.scene.control.ToggleGroup;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.input.MouseEvent;
import javafx.stage.FileChooser;
import javafx.stage.FileChooser.ExtensionFilter;
import javafx.stage.Stage;
import javafx.stage.StageStyle;

public class AddStudentController implements Initializable {
	private double xOffset = 0;
	private double yOffset = 0;
	@FXML
	private RadioButton maleRadio;
	@FXML
	private RadioButton femaleRadio;
	@FXML
	private ToggleGroup group;
	@FXML
	private TextField urlTF;
	@FXML
	private TextField codeTF;
	@FXML
	private TextField nameTF;
	@FXML
	
	private TextField phoneTF;
	@FXML
	private TextField emailTF;
	private String sex="";
	private TextField dateTF;
	@FXML
	private ChoiceBox<String> classChoiceBox;
	@FXML
	private ChoiceBox<String> hometownChoiceBox;
	@FXML
	private DatePicker datePicker;
	@FXML
	ImageView photo = new ImageView();
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
	@Override
	public void initialize(URL location, ResourceBundle resources) {
		ChoiceBoxData cbd = new ChoiceBoxData("jdbc:ucanaccess://\\JAVA\\Workspace\\652591_VuVanThuyen\\Database\\stdm.accdb", "", "");
		ObservableList<String> class_ = FXCollections.observableArrayList(cbd.getClassList());
		ObservableList<String> hometown = FXCollections.observableArrayList(cbd.getHomeTownList());
		classChoiceBox.setItems(class_);
		hometownChoiceBox.setItems(hometown);
		urlTF.textProperty().addListener(new ChangeListener<String>(){
			@Override
			public void changed(ObservableValue<? extends String> arg0, String arg1, String arg2) {
				String url = urlTF.getText();
				File file = new File(url);
				Image image = new Image(file.toURI().toString());
				photo.setImage(image);
			}
		
		});	
	}
	public static final LocalDate LOCAL_DATE(String dateString){
	    DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd-MM-yyyy");
	    LocalDate localDate = LocalDate.parse(dateString, formatter);
	    return localDate;
	}
	@FXML
	public void onClickBrowse() {
		Stage stage = (Stage) urlTF.getScene().getWindow();
		FileChooser fileChooser = new FileChooser();
		fileChooser.setInitialDirectory(new File("\\JAVA\\Workspace\\652591_VuVanThuyen\\img"));
		fileChooser.getExtensionFilters().addAll(new ExtensionFilter("Image Files", "*.png", "*.jpg", "*.gif"));
		File selectedFile = fileChooser.showOpenDialog(stage);
		 if (selectedFile != null) {
			 urlTF.setText(selectedFile.getAbsolutePath());
		 }
	}
	@FXML
	public void onClickClose(ActionEvent event) {
		((Node)(event.getSource())).getScene().getWindow().hide();
		showHomeScene();
	}
	@FXML
	public void onClickMinimize(ActionEvent event) {
		Stage stage = (Stage)((Node)(event.getSource())).getScene().getWindow();
		stage.setIconified(true);
	}
	
	public void showHomeScene() {
		try {
			Parent root = FXMLLoader.load(getClass().getResource("HomeScene.fxml"));
			Stage stage = new Stage();
			stage.initStyle(StageStyle.UNDECORATED);
			stage.setScene(new Scene(root));
			stage.show();
			
		} catch (Exception e) {
			ShowAlertDialog.showErrorAlert("Error","Cant't show HomeScene");
		}
	}
	
	@FXML
	public void onClickAddButton() throws ParseException {
		StudentManagement stdm = new StudentManagement("jdbc:ucanaccess://\\JAVA\\Workspace\\652591_VuVanThuyen\\Database\\stdm.accdb","","");
		try {
			sex = group.getSelectedToggle().equals(maleRadio)? "Nam" : "Nữ";
		} catch (Exception e) {
		}
		dateTF = (TextField)datePicker.getEditor();
		String class_=classChoiceBox.getSelectionModel().getSelectedItem();
		String hometown=hometownChoiceBox.getSelectionModel().getSelectedItem();
		
		
		
		if(checkDataValidation()) {
			Student std = new Student(codeTF.getText(),nameTF.getText(), sex, class_, dateTF.getText(), hometown, 
					  				  phoneTF.getText(), emailTF.getText(),urlTF.getText().substring(2));
			boolean addResult = stdm.addStudent(std);	
			if(addResult) {
				ShowAlertDialog.showSuccessfulAlert("Add Student", "Add to Database Successful!");
			}
			else {
				ShowAlertDialog.showErrorAlert("Add Student", "Add to Database Failed!");
			}
		}	
	}
	
	@FXML
	public void onClickCleanButton() {
		codeTF.setText("");
		urlTF.setText("");
		nameTF.setText("");
		maleRadio.setSelected(false);
		femaleRadio.setSelected(false);
		datePicker.setValue(null);
		classChoiceBox.setValue(null);
		hometownChoiceBox.setValue(null);
		phoneTF.setText("");
		emailTF.setText("");
	}
	
	public boolean checkDataValidation() {
		dateTF = (TextField)datePicker.getEditor();
		File file = new File(urlTF.getText());
		if("".equals(codeTF.getText())) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Invalid \"Mã Số\" field data!");
			return false;
		}
		if("".equals(nameTF.getText())) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Invalid \"Họ tên\" field data!");
			return false;
		}
		if("".equals(sex)) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Haven't chosen gender selected!");
			return false;
		}
		if("".equals(dateTF.getText())) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Haven't chosen date of birth!");
			return false;
		}
		if((classChoiceBox.getSelectionModel().getSelectedItem())==null) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Haven't chosen \"Lớp\"!");
			return false;
		}
		if(hometownChoiceBox.getSelectionModel().getSelectedItem()==null) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Haven't chosen \"Quê quán\"!");
			return false;
		}
		if("".equals(phoneTF.getText())) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Invalid \"Số điện thoại\" field data!");
			return false;
		}
		if("".equals(emailTF.getText())) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Invalid \"Email\" field data!");
			return false;
		}
		if("".equals(urlTF.getText())) {
			ShowAlertDialog.showWarningAlert("Invalid Form","Invalid \"URL\" field data!");
			return false;
		}
		if(!file.getAbsolutePath().endsWith(".jpg") 
		   && !file.getAbsolutePath().endsWith(".jpeg") && !file.getAbsolutePath().endsWith(".png")){
				ShowAlertDialog.showWarningAlert("Invalid Form", "Cant't load the Photo");
				return false;
		}
		return true;
	}
}
