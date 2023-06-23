package gui;

import core.ACCM;
import core.AdminAccount;
import core.ShowAlertDialog;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Node;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.stage.Stage;
import javafx.stage.StageStyle;

public class LoginController {
	private double xOffset = 0;
	private double yOffset = 0;
	@FXML
	private TextField usernameTF;
	@FXML
	private PasswordField passwordTF;
	@FXML
	private Label message;
	
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
	public void onClickExit(ActionEvent event) {
		System.exit(0);
	}
	@FXML
	public void onClickMinimize(ActionEvent event) {
		Stage stage = (Stage)((Node)(event.getSource())).getScene().getWindow();
		stage.setIconified(true);
	}
	public boolean validateForm() {
		message.setText("");
		if("".equals(usernameTF.getText()) || "".equals(passwordTF.getText())) {
			message.setText("Invalid Login: Please try again!");
			return false;
		}
		return true;
	}
	@FXML
	public void onClickLoginButton(ActionEvent event){
		if(validateForm()) {
			ACCM accm = new ACCM("jdbc:ucanaccess://\\JAVA\\Workspace\\652591_VuVanThuyen\\Database\\stdm.accdb","","");
			AdminAccount acc = new AdminAccount(usernameTF.getText(), passwordTF.getText());
			{
				if(accm.checkAccount(acc)) {
					ShowAlertDialog.showSuccessfulAlert("Login", "Login Successful");
					((Node)(event.getSource())).getScene().getWindow().hide();
					showTableView();
				}
				else {
					ShowAlertDialog.showErrorAlert("Login", "Username or Password is incorrect!");
				}
			}
		}	
	}

	public void showTableView() {
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
	
	
}
