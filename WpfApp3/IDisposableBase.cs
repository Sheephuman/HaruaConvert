using System;
using System.IO;

#pragma warning disable 0649

namespace WpfApp3
{

    internal class IDisposableBase :IDisposable
    {
      

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                this.Free(this.unmanagedResource);

                if (isDisposing)
                {
                    if (this.managedResource != null)
                    {
                        this.managedResource.Dispose();
                    }
                }

                this.disposed = true;
            }
        }


        private readonly IntPtr unmanagedResource;


        public bool disposed;
        private StreamWriter managedResource;

        private void Free(IntPtr unmanagedResource)
        {
            //  MessageBox.Show("CAｌｌ");       

        }

     
    }
}
