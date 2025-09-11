class CTListForm extends CTBaseForm {

    grid: CTGridData = null;

    loadData() {
        let me = this;
        if (me.grid) {
            me.grid.loadData();
        }

    }
}
