// =============================================================================
// =
// =   FILE:			VideoDecoder.cs
// =
// =============================================================================
// =                                                                        
// = COPYRIGHT: Title to, ownership of and copyright of this software is
// = vested in Copyright 2003-2014  Baker Hughes Reservoir Software BV.
// = is a wholly-owned subsidiary of Baker Hughes Incorporated.
// = All rights reserved.
// =
// = Neither the whole nor any part of this software may be
// = reproduced, copied, stored in any retrieval system or
// = transmitted in any form or by any means without prior
// = written consent of the copyright owner.
// =
// = This software and the information and data it contains is
// = confidential. Neither the whole nor any part of the
// = software and the data and information it contains may be
// = disclosed to any third party without the prior written
// = consent of Copyright 2003-2014 Baker Hughes Reservoir Software BV, a
// = wholly-owned subsidiary of Baker Hughes Incorporated 
// =                                                                          
// =============================================================================

using System.Diagnostics;
using AR.Drone.Infrastructure;
using AR.Drone.Video.Exceptions;
using FFmpeg.AutoGen;

namespace AR.Drone.Video
{
    public unsafe class VideoDecoder : DisposableBase
    {
        private const AVCodecID CodecId = AVCodecID.AV_CODEC_ID_H264;
        private readonly AVCodecContext* _pDecodingContext;
        private readonly AVFrame* _pFrame;

        static VideoDecoder()
        {
            FFmpegInvoke.avcodec_register_all();
        }

        public VideoDecoder()
        {
            _pFrame = FFmpegInvoke.av_frame_alloc();

            AVCodec* pCodec = FFmpegInvoke.avcodec_find_decoder(CodecId);

            if (pCodec == null)
                throw new VideoDecoderException("Unsupported codec.");

            _pDecodingContext = FFmpegInvoke.avcodec_alloc_context3(pCodec);


            if (FFmpegInvoke.avcodec_open2(_pDecodingContext, pCodec, null) < 0)
                throw new VideoDecoderException("Could not open codec.");
        }

        public bool TryDecode(ref AVPacket packet, out AVFrame* pFrame)
        {
            int gotPicture;
            fixed (AVPacket* pPacket = &packet)
            {
                int decodedSize = FFmpegInvoke.avcodec_decode_video2(_pDecodingContext, _pFrame, &gotPicture, pPacket);
                if (decodedSize < 0)
                    Trace.TraceWarning("Error while decoding frame.");
            }
            pFrame = _pFrame;
            return gotPicture == 1;
        }


        protected override void DisposeOverride()
        {
            FFmpegInvoke.avcodec_close(_pDecodingContext);

            AVFrame* frameOnStack = _pFrame;
            AVFrame** frame = &frameOnStack;
            FFmpegInvoke.av_frame_free(frame);
        }
    }
}