<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(COL4,'CALL') or contains(COL4,'PUT')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,'1')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8) div 1000,'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>-->
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
      <!--</xsl:otherwise>-->
      <!--

			</xsl:choose>-->
    </xsl:if>
  </xsl:template>
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL16"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) and COL9='STK'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Interactive Brokers'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>



			  <xsl:variable name="Asset">
              <xsl:choose>



                <xsl:when test="contains(COL9,'OPT')">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>

                
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <!--<AssetType>
              <xsl:value-of select="$Asset"/>
            </AssetType>-->

            <xsl:variable name="Symbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

			  <xsl:variable name="PB_SUFFIX_NAME">
				  <xsl:value-of select="COL10"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SUFFIX_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
			  </xsl:variable>

            <Symbol>


              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
				  <xsl:when test="$Asset='Option'">
					  <xsl:value-of select="''"/>
				  </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>



            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>


                <xsl:when test="$Asset='Option'">
                  <xsl:value-of select="concat(COL7,'U')"/>
                </xsl:when>

				  <xsl:when test="$Symbol!='*'">
					  <xsl:value-of select="''"/>
				  </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>

            <xsl:variable name="PB_FUND_NAME" select="''"/>

			  <xsl:variable name ="PRANA_FUND_NAME">
				  <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

			  <FundName>
				  <xsl:choose>

					  <xsl:when test ="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select ="$PRANA_FUND_NAME"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_FUND_NAME"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </FundName>



			  <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="COL9">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9 "/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="CostBasis" select="(COL17)"/>



            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>







            <xsl:variable name="Side" select="COL15"/>

            <SideTagValue>


				<xsl:choose>
					<xsl:when test="$NetPosition &gt; 0">
						<xsl:value-of select="'1'"/>
					</xsl:when>
					<xsl:when test="$NetPosition &lt; 0">
						<xsl:value-of select="'2'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>

                           
            </SideTagValue>

			  <xsl:variable name="varFXrate">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL27"/>
				  </xsl:call-template>
			  </xsl:variable>
			  <FXRate>
				  <xsl:choose>
					  <xsl:when test="number($varFXrate)">
						  <xsl:value-of select="$varFXrate"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </FXRate>

            <!--<SEDOL>
              <xsl:value-of select="COL9" />
            </SEDOL>-->

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


           


            <xsl:variable name="Year1" select="substring(COL12,1,4)"/>
            <xsl:variable name="Month" select="substring(COL12,5,2)"/>
            <xsl:variable name="Day" select="substring(COL12,7,2)"/>



            <PositionStartDate>

             <xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>
             
            </PositionStartDate>




			  <xsl:variable name="varYear1" select="substring(COL14,1,4)"/>
			  <xsl:variable name="varMonth" select="substring(COL14,5,2)"/>
			  <xsl:variable name="varDay" select="substring(COL14,7,2)"/>
			  <PositionSettlementDate>              
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear1)"/>
            </PositionSettlementDate>


         

            <xsl:variable name="COL19">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL20">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL20 "/>
              </xsl:call-template>
            </xsl:variable>
			  
            <xsl:variable name="COL33">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL47">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL47 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL48">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL48 "/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="COL49">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL49 "/>
              </xsl:call-template>
            </xsl:variable>

			  <!--<xsl:variable name="Commission" select="($COL44 + $COL45 + $COL46 + $COL47 + $COL48 + $COL49)"/>-->
			  <xsl:variable name="Commission" select="$COL20 "/>
			  <xsl:variable name="varCommission">
				  <xsl:choose>
					  <xsl:when test="COL33= 0">
						  <xsl:value-of select="$COL20"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$COL33"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>

                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Commission>


			  <xsl:variable name="varFee">
				  <xsl:choose>
					  <xsl:when test="COL33!= 0">
						  <xsl:value-of select="$COL20"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <TransactionLevy>
				  <xsl:choose>
					  <xsl:when test="$varFee &gt; 0">
						  <xsl:value-of select="$varFee"/>

					  </xsl:when>
					  <xsl:when test="$varFee &lt; 0">
						  <xsl:value-of select="$varFee * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </TransactionLevy>

						<Strategy>
				  <xsl:value-of select="COL15"/>
			  </Strategy>

			  <Coupon>
				  <xsl:value-of select="COL22"/>
			  </Coupon>




          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>