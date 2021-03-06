﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HotelApp1.Model;
using System.Collections.ObjectModel;

namespace HotelApp1.Persistency
{
    class PersistencyService
    {
        // const string serverUrl = "http://webservicehotel20170329012418.azurewebsites.net";
        const string serverUrl = "http://localhost:19611/";
        static HttpClientHandler clientHandler = new HttpClientHandler();
        static HttpClient client = new HttpClient(clientHandler);

        public PersistencyService()
        {

        }

        #region Get all guests
        public static async Task<ObservableCollection<Guest>> GetAllGuests()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                var response = await client.GetAsync("api/guests");

                if (response.IsSuccessStatusCode)
                {
                    var allGuestsList = await response.Content.ReadAsAsync<ObservableCollection<Guest>>();
                    return allGuestsList;
                }
                return null;
            }
        }
        #endregion

        #region Get one guest
        public static async Task<Guest> GetOneGuest(int guestid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                var response = await client.GetAsync("api/guests/" + guestid);

                if (response.IsSuccessStatusCode)
                {
                    var guest = await response.Content.ReadAsAsync<Guest>();
                    return guest;
                }
                return null;
            }
        }
        #endregion

        #region Post new guest
        public static async Task<bool> AddOneGuest(Guest newguest)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                var response = await client.PostAsJsonAsync<Guest>("api/guests", newguest);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Put guest
        public static async Task<bool> EditGuest(Guest guestid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                string putUrl = "api/guests/" + guestid.Guest_No.ToString();

                var response = await client.PutAsJsonAsync<Guest>(putUrl, guestid);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Delete guest
        public static async Task<bool> DeleteOneGuest(Guest guestid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                string deleteUrl = "api/guests/" + guestid.Guest_No.ToString();

                var response = await client.DeleteAsync(deleteUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region ViewGuestBooking
        public static ObservableCollection<GuestBooking> FindGuestBooking(int guestid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                string bookingUrl = "api/guestbooking/";
                
                var bresponse = client.GetAsync(bookingUrl).Result;

                if (bresponse.IsSuccessStatusCode)
                {
                    var BookingById = new ObservableCollection<GuestBooking>();

                    foreach (var item in bresponse.Content.ReadAsAsync<ObservableCollection<GuestBooking>>().Result) 
                    {
                        if (item.bGuestId == guestid)
                        {
                            BookingById.Add(item);
                        }
                    }
                    return BookingById;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
