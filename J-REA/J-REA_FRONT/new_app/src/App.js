import React, { useState, useEffect } from "react";
import "./App.css";

const Solicitud = ({ id, rol, usuario, estado, onLogin }) => {
    const [selectedValue, setSelectedValue] = useState("0");
    const [descripcion, setDescripcion] = useState("");
    const [justificativo, seJustificativo] = useState("");
    const [fileName, setFileName] = useState(""); 
    const [esNuevo, setEsNuevo] = useState(true); 
    const [estadoNuevo, setEstado] = useState(estado); 
    const [error, setError] = useState(null);

    const handleSelectChange = (event) => {
        setSelectedValue(event.target.value);
    };

    var uri = "http://localhost:5032/";

    if(id > 0) {
        setEsNuevo(false);
        consultar();
    } 

    const consultar = async () => {
        try {
            const response = await fetch( uri + "Usuario/"+ id, {
                method: "GET",
                headers: { "Content-Type": "application/json" },
            });
  
            const data = await response.json();
            if (!response.ok) {
                throw new Error(data.mensajeError);
            }

            setSelectedValue(data.data.Tipo);
            setDescripcion(data.data.Descripcion);
            seJustificativo(data.data.Justificativo.split(" ")[1]);
            setFileName(data.data.Justificativo.split(" ")[0]);
            setEstado(data.data.Estado);

        } catch (error) {
            setError(error.message);
        }
    };
    
    const crear = async (e) => {
        e.preventDefault();
  
        try {
            const response = await fetch( uri + "Solicitud/Ingresar", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(
                    { 
                        Tipo: selectedValue, 
                        Descripcion: descripcion, 
                        Justificativo: fileName +" "+ justificativo 
                    }),
            });
  
            const data = await response.json();
            if (!response.ok) {
                throw new Error(data.mensajeError);
            }
  
            onLogin(rol, true, usuario);
        } catch (error) {
            setError(error.message);
        }
    };

    const actualizar = async (e) => {
        e.preventDefault();
  
        try {
            const response = await fetch( uri + "Solicitud/Actualizar"+ id, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(
                    { 
                        Tipo: selectedValue, 
                        Descripcion: descripcion, 
                        Justificativo: fileName +" "+ justificativo 
                    }),
            });
  
            const data = await response.json();
            if (!response.ok) {
                throw new Error(data.mensajeError);
            }
  
            onLogin(rol, true, usuario);
        } catch (error) {
            setError(error.message);
        }
    };

    const submit = (e) => {
        e.preventDefault();
        if (esNuevo) {
            crear(); 
        } else {
            actualizar();
        }
    };
  
  
    return (
        <div>
            {id = 0 ? <h2>Nueva solicitud</h2>: ""}
            {id > 0 ? <h2>Actualizar solicitud</h2>: ""}
            {rol != "cliente" ? <h2>Cerrar solicitud</h2>: ""}
            <form onSubmit={submit}>
                <div>
                    <label>Tipo:</label>
                    <select value={selectedValue} 
                            onChange={handleSelectChange} 
                            disabled={(rol != "cliente" || estadoNuevo != "INGRESADO") ? true : false}>
                        <option value="0">Seleccione una opción</option>
                        <option value="REQUERIMIENTO">REQUERIMIENTO</option>
                        <option value="INFORMACION">INFORMACION</option>
                    </select>
                </div>
                <div>
                    <label>Descripcion:</label>
                    <input
                        type="text"
                        value={descripcion}
                        onChange={(e) => setDescripcion(e.target.value)}
                        disabled={(rol != "cliente" || estadoNuevo != "INGRESADO") ? true : false}
                    />
                </div>
                <div>
                    <label>Estado: {estadoNuevo}</label>
                </div>
                { rol != "cliente" && (
                    <>
                        
                    </>
                )}

                <button type="submit">Enviar</button>
            </form>
        </div>
    );
};

const Cerrar = ({ id, rol, usuario, estado, detalle, onLogin }) => {
    const [error, setError] = useState(null);
    var uri = "http://localhost:5032/";
  
    const terminar = async () => {
        try {
            const response = await fetch( uri + "Solicitud/Cerrar" + id, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(
                    { 
                        Estado: estado, 
                        DetalleGestion: detalle 
                    }
                ),
            });
  
            const data = await response.json();
            if (!response.ok) {
                throw new Error("Error");
            }
  
            onLogin(rol, true, usuario);
        } catch (error) {
            setError(error.message);
        }
    };
    useEffect(() => {
        terminar();
    }, []);
};

const Solicitudes = ({id, rol, usuario }) => {
  const [solicitudes, setSolicitudes] = useState([]);
  const [error, setError] = useState(null);

  var uri = "http://localhost:5032/";

  const fetchSolicitudes = async () => {
      try {
          const response = await fetch(uri + "Solicitud/listar", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Usuario: usuario, Rol: rol, Estado: "", FechaIngreso: "", ClienteId: 0})
          });

          if (!response.ok) {
              throw new Error("Error al obtener las solicitudes");
          }
          const data = await response.json();
          setSolicitudes(data.data);
        } catch (error) {
          setError(error.message);
        }
    };

    useEffect(() => {
        fetchSolicitudes();
    }, []);

    if (error) {
      return <p>Error: {error}</p>;
    }

    return (
      <div>
          {rol == "cliente" ? <button>Nueva Solicitud</button> : ""}
          <h2>Listado de Solicitudes</h2>
          <table>
                <tr>
                    <td>Solicitud</td>
                    <td>Cliente</td>
                    <td>Tipo</td>
                    <td>FechaIngreso</td>
                    <td>FechaIngreso</td>
                    <td>Actualizar</td>
                    <td>Enviar</td>
                    <td>Cerrar</td>
                </tr>
                {solicitudes.map((solicitud) => (
                  <tr>
                    <td>{solicitud.SolicitudId}</td>
                    <td>{solicitud.Cliente}</td>
                    <td>{solicitud.Tipo}</td>
                    <td>{solicitud.FechaIngreso}</td>
                    <td>{solicitud.estado == "INGRESADO" ? <button>Actualizar</button> : ""}</td>
                    <td>{solicitud.estado == "INGRESADO" ? <button>Enviar</button> : <button>Ver</button>}</td>
                    <td>{rol != "cliente" ? <button>Responder</button> : ""}</td>
                  </tr>
              ))}
                
          </table>
      </div>
    );
};

function App() {
  const [isValid, setIsValid] = useState(true);
  const [rol, setRol] = useState("cliente");
  const [usuario, setUsuario] = useState("cliente1");

    const handleLogin = (userRol, isValidd, userName) => {
        setRol(userRol);
        setIsValid(isValidd);
        setUsuario(userName);
    };

    return (
        <div>
            {isValid ? (
                <Solicitudes rol={rol} usuario={usuario} />
            ) : (
                <></>
            )}
        </div>
    );
}

export default App;