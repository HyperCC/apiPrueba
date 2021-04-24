import { createMuiTheme } from '@material-ui/core/styles';

//tema principal para trabajar en el proyecto completo
const theme = createMuiTheme({
    palette: {
        primary: {
            light: "#63a4fff",
            main: "#1976d2",
            dark: "#004ba0",
            contrastText: "#ecfad8"
        }
    }
});

export default theme;