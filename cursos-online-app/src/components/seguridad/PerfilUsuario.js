import React from 'react';
import style from '../Tools/Style';
import { Button, Container, Grid, TextField, Typography } from '@material-ui/core';

const PerfilUsuario = () => {
    return (
        <Container>
            <div style={style.paper}>
                <Typography component="h1" variant="h5">
                    Perfil de Usuario
                </Typography>

                <form style={style.form}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} md={6}>
                            <TextField name="nombrecompleto" variant="outlined" fullWidth label="Ingrese nombre y apellidos" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="email" variant="outlined" fullWidth label="Ingrese email" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="password" type="password" variant="outlined" fullWidth label="Ingrese password" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="confirmepassword" type="password" variant="outlined" fullWidth label="Confirme password" />
                        </Grid>
                    </Grid>

                    <Grid container justify="center">
                        <Grid item xs={12} md={6}>
                            <Button type="submit" fullWidth variant="contained" size="large" color="primary" style={style.submit}>
                                Guardar Datos
                        </Button>
                        </Grid>
                    </Grid>
                </form>
            </div>
        </Container>
    );
};

export default PerfilUsuario;