import React from 'react';
// obtener todos todos los objetos de material design
import { ThemeProvider as MuithemeProvider } from '@material-ui/core/styles';
import theme from './theme/theme';
import PerfilUsuario from './components/seguridad/PerfilUsuario';

function App() {
  return (
    <MuithemeProvider theme={theme}>
      <PerfilUsuario />
    </MuithemeProvider>
  );
}

export default App;
