<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Prooveedores.aspx.cs" Inherits="Gestion_Web.Formularios.Proveedores.Prooveedores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

    <div class="container">

      <div class="row">
      	
      	<div class="col-md-6 col-xs-12">
      		
      		<div class="widget stacked widget-table action-table">
					
				<div class="widget-header">
					<i class="icon-th-list"></i>
					<h3>Proveedores</h3>
				</div> <!-- /widget-header -->
				
                  
				<div class="widget-content">
                   <%-- <table>
                        <tr>
                            <td>
                                <<div class="form-group">
                                    <input type="text" class="form-control input-sm search-query" placeholder="Search">
                                </div>
                        </td>            
                       </tr>
                  </table>   --%> 
            
					<table  id="tbClientes" class="table table-striped table-bordered">
						<thead>
							<tr>
								<th style="width: 40%">Razon Social</th>
                                <th style="width: 40%">Alias</th>
								<th class="td-actions" style="width: 20%"></th>
							</tr>
						</thead>
						<tbody>
                            <asp:PlaceHolder ID="phClientes" runat="server"></asp:PlaceHolder>
							</tbody>
						</table>
					
				</div> <!-- /widget-content -->
			
			</div> <!-- /widget -->
			
			
			</div>

          
			
					<div class="col-md-6">
						
                        
            <div class="widget stacked">
				
				<div class="widget-header">
					<i class="icon-wrench"></i>
					<h3>Herramientas</h3>
				</div> <!-- /widget-header -->
				
				<div class="widget-content">
					<table style="width: 100%">
                        <tr>
                            <td style="width: 70%">
                                <div class="col-md-8">
								<div class="input-group">
                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Cliente"></asp:TextBox>
							      <span class="input-group-btn">
                                      <asp:Button ID="btnBuscar" runat="server" Text="Ir!" class="btn btn-primary" />
							        <%--<button class="btn btn-primary" type="button">Buscar!</button>--%>
							      </span>
							    </div><!-- /input-group -->



							</div>
                            </td>
                            
                            <td style="width: 10%">

                                <a href="ProoveedoresABM.aspx" class="btn btn-default" style="width:100%">
                                    <i class="shortcut-icon icon-plus"></i>
                                    
                                </a>

                            </td>

                            <td style="width: 10%">
                                <a href="/" class="btn btn-default" runat="server" id="linkEditar" style="width: 100%">
                                    <i class="shortcut-icon icon-pencil"></i>
                                </a>

                            </td>

                        
                      </tr>
					</table>
				</div> <!-- /widget-content -->
			
			</div> <!-- /widget -->
                        				
			<div class="widget stacked">
				
				<div class="widget-header">
					<i class="icon-file"></i>
					<h3>Detalle</h3>
				</div> <!-- /widget-header -->
				
				<div class="widget-content">
					<fieldset>
                    
        
                    
						   <%-- <div class="form-group">
						      <label for="name" class="col-lg-4">Codigo Cliente</label>

								<%--<div class="col-lg-8">
                                    
                                    <asp:TextBox ID="txtCodCliente" runat="server" class="form-control" ></asp:TextBox>
                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
						 
								<%--</div>--%>
                        <div class="plan-features" >
							
                            <ul>
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                    
                                    <asp:PlaceHolder ID="phClienteDetalle" runat="server"></asp:PlaceHolder>
                                   
                                    
                                 
									<%--<li style="text-align:left"><strong >Codigo Cliente:</strong></li>
									<li style="text-align:left"><strong>Razon Social:</strong><asp:Label ID="LaRazonSocial" runat="server" Text="ddd"></asp:Label></li>
									<li style="text-align:left"><strong>Alias:</strong><asp:Label ID="LaAlias" runat="server" Text="ddd"></asp:Label></li>
                                    <li style="text-align:left"><strong>Grupo:</strong><asp:Label ID="LaGrupo" runat="server" Text="dd"></asp:Label></li>
									<li style="text-align:left"><strong>Cuit:</strong> <asp:Label ID="LaCuit" runat="server" Text="dd" ></asp:Label></li>
									<li style="text-align:left"><strong>Iva:</strong> <asp:Label ID="LaIva" runat="server" Text=""></asp:Label></li>--%>
								</ul>
                               
							</div> <!-- /plan-features -->
                        
						    </div>
                        
                            <%--<div class="form-group">
						      <label for="name" class="col-lg-4">Razon Social</label>

								<div class="col-lg-8">
                                    
                                    <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control" ></asp:TextBox>
						        	<%--<input type="text" class="form-control" name="name" id="name">--%>
						 
								<%--</div>
						    </div>
                            
                            <div class="form-group">
						      <label for="name" class="col-lg-4">Alias</label>

								<div class="col-lg-8">
                                    
                                    <asp:TextBox ID="txtAlias" runat="server" class="form-control" ></asp:TextBox>
						        	<%--<input type="text" class="form-control" name="name" id="name">
						 
								</div>
						    </div>
                             
                            <div class="form-group">
				            <label for="validateSelect" class="col-lg-4">Grupo</label>
							<div class="col-lg-8">
                                <asp:TextBox ID="txtGrupo" runat="server" class="form-control" ></asp:TextBox>
				              
				              </div>
				          </div>
                                 
                                               
                           <%--<br /> 
                            <div class="form-group">
				            <label for="validateSelect" class="col-lg-4">Pais</label>
							<div class="col-lg-8">
                                <asp:TextBox ID="txtPais" runat="server" class="form-control"></asp:TextBox>
				              
				              </div>
				          </div>
                            <div class="form-group">
						      <label for="name" class="col-lg-4">Cuit</label>

								<div class="col-lg-8">
                                    
                                    <asp:TextBox ID="txtCuit" runat="server" class="form-control" ></asp:TextBox>
						        	<%--<input type="text" class="form-control" name="name" id="name">
						 
								</div>
						    </div>
                        
                            <div class="form-group">
				            <label for="validateSelect" class="col-lg-4">Iva</label>
							<div class="col-lg-8">
                                <asp:TextBox ID="txtIva" runat="server" class="form-control" ></asp:TextBox>
				              
				              </div>
				          </div>--%>
                           
						  </fieldset>
					
				</div> <!-- /widget-content -->
			
			</div> <!-- /widget -->
      		
	    
      	
      	
      	
      	
      </div> <!-- /row -->

    </div> <!-- /container -->
    
</div> <!-- /main -->

</asp:Content>
