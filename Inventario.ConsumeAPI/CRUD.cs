using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System;

namespace Inventario.ConsumeAPI
{
    public static class CRUD<T>
    {
        public static T Created(string urlApi, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = client.SendAsync(request);
                    response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }
        public static T[] Read(string urlApi)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetStringAsync(urlApi);
                    response.Wait();

                    var json = response.Result;
                    var result = JsonConvert.DeserializeObject<T[]>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("La API está cargando o ha ocurrido un error", ex);
            }
        }
        public static T Read_ById(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetStringAsync(urlApi + "/" + id);
                response.Wait();

                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }

        }

        public static T Read_Token(string urlApi, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                // Agregar el encabezado de autorización
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Realizar la solicitud HTTP
                var response = client.GetStringAsync(urlApi);
                response.Wait();

                // Leer la respuesta JSON
                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        //public static T Read_Token_getROL(string urlApi, string token, string id)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        // Agregar el encabezado de autorización
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //        // Realizar la solicitud HTTP
        //        var response = client.GetStringAsync(urlApi+ "/" + id);
        //        response.Wait();

        //        // Leer la respuesta JSON
        //        var json = response.Result;
        //        var result = JsonConvert.DeserializeObject<T>(json);
        //        return result;
        //    }
        //}
        public static RoleResponse Read_Token_getROL(string urlApi, string token, string id)
        {
            using (HttpClient client = new HttpClient())
            {
                // Agregar el encabezado de autorización
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Realizar la solicitud HTTP
                var response = client.GetStringAsync(urlApi + "/" + id);
                response.Wait();

                // Leer la respuesta JSON
                var json = response.Result;
                var result = JsonConvert.DeserializeObject<RoleResponse>(json);
                return result;
            }
        }


        public static async Task<List<T>> Read_ByIdSQLAsync(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(urlApi + "/" + id);
                    var result = JsonConvert.DeserializeObject<List<T>>(response);
                    return result;
                }
                catch (HttpRequestException ex)
                {
                    // Aquí puedes manejar el error según tus necesidades
                    // Por ejemplo, podrías lograr el error, redirigir a una página de error, o retornar una lista vacía
                    Console.WriteLine($"Error al consultar la API: {ex.Message}");
                    return new List<T>(); // Retorna una lista vacía o null, dependiendo de tu lógica
                }
            }
        }
        public static T Read_lastcod(string urlApi)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetStringAsync(urlApi);
                response.Wait();

                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }



        public static bool Update(string urlApi, int id, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Put, urlApi + "/" + id);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool Delete(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var request = new HttpRequestMessage(HttpMethod.Delete, urlApi + "/" + id);

                var response = client.SendAsync(request);
                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static T Login(string urlApi, string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var loginData = new
                {
                    usr_user = username,
                    usr_password = password
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }
        public static T Login2(string urlApi, string username, string password, string modulo)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var loginData = new
                {
                    usr_user = username,
                    usr_password = password,
                    mod_name= modulo
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }


    }
}

