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
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="string-length(COL10) &gt; 20">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <xsl:variable name ="Symbol" select="normalize-space(COL6)"/>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="COL9!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Symbol!=''">
						  <xsl:value-of select="$Symbol"/>
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
						  <xsl:value-of select="concat(COL10,'U')"/>
					  </xsl:when>
					  <xsl:when test="COL9!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Symbol!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					 
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </IDCOOptionSymbol>
			  
			  <SEDOL>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="COL9!=''">
						  <xsl:value-of select="COL9"/>
					  </xsl:when>
					 
					  <xsl:when test="$Symbol!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					 
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </SEDOL>





			  <xsl:variable name="PB_FUND_NAME" select="number(COL3)"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <AccountName>
				  <xsl:choose>

					  <xsl:when test ="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select ="$PRANA_FUND_NAME"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select ="$PB_FUND_NAME"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </AccountName>



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


            <xsl:variable name="CostBasis" select="COL16"/>



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

            <xsl:variable name="Side" select="normalize-space(COL2)"/>

            <SideTagValue>

				<xsl:choose>
					<xsl:when test="$Asset='Option'">
						<xsl:choose>
							<xsl:when test ="$Side='Buy'">
								<xsl:value-of select ="'A'"/>
							</xsl:when>
							<xsl:when test ="$Side='Short'">
								<xsl:value-of select ="'C'"/>
							</xsl:when>
							<xsl:when test ="$Side='Cover'">
								<xsl:value-of select ="'B'"/>
							</xsl:when>
							<xsl:when test ="$Side='Sell'">
								<xsl:value-of select ="'D'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test ="$Side='Buy' and normalize-space(COL5)='STOCK'">
								<xsl:value-of select ="'1'"/>
							</xsl:when>
							<xsl:when test ="$Side='Sell' and normalize-space(COL5)='STOCK'">
								<xsl:value-of select ="'2'"/>
							</xsl:when>
							<xsl:when test ="$Side='Short' and normalize-space(COL5)='STOCK'">
								<xsl:value-of select ="'5'"/>
							</xsl:when>
							<xsl:when test ="$Side='Cover' and normalize-space(COL5)='STOCK'">
								<xsl:value-of select ="'B'"/>
							</xsl:when>
							<xsl:when test ="$Side='Sell'">
								<xsl:value-of select ="'2'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</SideTagValue>

			
            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>


            <xsl:variable name ="Date" select="COL15"/>


            <xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>



            <PositionStartDate>

              <!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
				<xsl:value-of select="COL12"/>

            </PositionStartDate>

            <xsl:variable name ="Date1" select="COL16"/>


            <xsl:variable name="Year" select="substring($Date1,1,4)"/>
            <xsl:variable name="Month1" select="substring($Date1,5,2)"/>
            <xsl:variable name="Day1" select="substring($Date1,7,2)"/>



            <PositionSettlementDate>

              <!--<xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>-->
				<xsl:value-of select="COL13"/>
            </PositionSettlementDate>




            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL18"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>

              <xsl:choose>

                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>

                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Commission>

            <xsl:variable name="StampDuty">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL"/>
              </xsl:call-template>
            </xsl:variable>

			  <SecFees>

              <xsl:choose>

                <xsl:when test="$StampDuty &gt; 0">
                  <xsl:value-of select="$StampDuty"/>
                </xsl:when>

                <xsl:when test="$StampDuty &lt; 0">
                  <xsl:value-of select="$StampDuty * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </SecFees>

            <xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>

            <Fees>

              <xsl:choose>

                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>
                </xsl:when>

                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Fees>

            <CurrencySymbol>
              <xsl:value-of select="COL14"/>
            </CurrencySymbol>


            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>


          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>