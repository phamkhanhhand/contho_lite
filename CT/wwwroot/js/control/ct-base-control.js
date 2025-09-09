 /*
  * Tạo base control
  * phamha Nov 24, 2024
  * */

class KHBaseControl {

    constructor(id) {
        if (id) {
            this.htmlID = id;
        }
        else {
            this.htmlID = PKHA.commonFn.uuidv4();
        }

        this.initControl();
    }

    initControl() {

    }

    hide() {
        $('#' + this.htmlID).hide();
    }
    /*
     * Hiện
     * */
    show() {
        $('#' + this.htmlID).show();
    }

    /*
     * Đặt active
     * phamkhanhhand 06.11.2022
     * */
    setEnable(enable) {
        let me = this;

        if (enable) {
            this.show();
        }
        else {
            this.hide();
        }

    }




    getValue() {
        return null;
    }
}
 