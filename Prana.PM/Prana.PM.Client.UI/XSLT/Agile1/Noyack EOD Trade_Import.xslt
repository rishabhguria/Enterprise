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
    <xsl:if test="string-length(COL9) &gt;14">
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
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8),'#.00')"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
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

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL15"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="COL9"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="COL2='Options'">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->

            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="string-length(COL9) &gt;14">
                  <xsl:value-of select="'Option'"/>
                </xsl:when>


                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

			  <!--<xsl:variable name="PB_SUFFIX" select="COL2"/>


			  <xsl:variable name="PRANA_SUFFIX_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX]/@TickerSuffixCode"/>
			  </xsl:variable>-->


            <!--<xsl:variable name="Symbol" select="normalize-space(COL2)"/>-->
            <xsl:variable name="Symbol" select="substring-before(COL9,' ')"/>
              <!--<xsl:choose>
                <xsl:when test="contains(COL9,' ')">
                  <xsl:value-of select="substring-before(COL9,' ')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL9)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->
			  <Symbol>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>

					  <xsl:when test="string-length(COL9) &gt;12">
						  <xsl:call-template name="Option">
							  <xsl:with-param name="Symbol" select="COL9"/>
						  </xsl:call-template>
					  </xsl:when>


					  <xsl:when test="$Asset='Equity'">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>



			  </Symbol>


			  <!--<IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Asset='Equity Option'">
						  <xsl:value-of select="concat(COL9,'U')"/>


					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>-->



			  <xsl:variable name="PB_FUND_NAME" select="'Noyack Creek LP'"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>

                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetPosition>

            <xsl:variable name="Side" select="COL6"/>

            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Asset='Option'">
                  <xsl:choose>
                    <xsl:when test="$Side='BY'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SL'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SS'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>

					  <xsl:when test="$Side='CS'">
						  <xsl:value-of select="'B'"/>
					  </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>



                <xsl:otherwise>


                  <xsl:choose>
                    <xsl:when test="$Side='BUY'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SELL'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SS'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>

					  <xsl:when test="$Side='CS'">
						  <xsl:value-of select="'B'"/>
					  </xsl:when>

                       <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>


                </xsl:otherwise>

              </xsl:choose>



            </SideTagValue>


            <xsl:variable name="TradePrice">

              <xsl:value-of select="COL16"/>

            </xsl:variable>

            <CostBasis>
              <xsl:choose>

                <xsl:when test="$TradePrice &gt; 0">
                  <xsl:value-of select="$TradePrice"/>
                </xsl:when>

                <xsl:when test="$TradePrice &lt; 0">
                  <xsl:value-of select="$TradePrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <!--<xsl:variable name="Date" select="COL1"/>

            <PositionStartDate>
              <xsl:value-of select="$Date"/>
            </PositionStartDate>-->

            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15 * COL20"/>
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

           

            <!--<xsl:variable name="FXRate">
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="number($FXRate)">
                  <xsl:value-of select="$FXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>-->


			  <!--<xsl:variable name="PB_BROKER_NAME">
				  <xsl:value-of select="normalize-space(COL12)"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_BROKER_ID">
				  <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
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
			  </CounterPartyID>-->

            <xsl:variable name="SecFee">
              <xsl:choose>
                <xsl:when test="(contains(COL6,'SELL')or contains(COL6,'SS')) and $Asset='Equity'">
                  <xsl:value-of select="format-number($Position * $TradePrice * 0.0000218,'#.##')"/>
                </xsl:when>

                <xsl:when test="(contains(COL6,'SL')or contains(COL6,'SS')) and $Asset='Option'">
                  <xsl:value-of select="format-number($Position * $TradePrice * 100 * 0.0000218,'#.##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:variable>


            <StampDuty>

              <xsl:choose>
                <xsl:when test="$SecFee &gt; 0">
                  <xsl:value-of select="$SecFee"/>

                </xsl:when>
                <xsl:when test="$SecFee &lt; 0">
                  <xsl:value-of select="$SecFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </StampDuty>


            <!--<xsl:variable name="NetNotional">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>





            <xsl:variable name="Fees">
				<xsl:choose>
          <xsl:when test="($Side='BY' or $Side='CS') and $Asset='Equity'">
            <xsl:value-of select="format-number(($NetNotional - $Position * $TradePrice - $Commission),'0.####') "/>
          </xsl:when>


          <xsl:when test="($Side='BY' or $Side='CS') and $Asset='Option'">
            <xsl:value-of select="format-number(($NetNotional - ($Position * 100) * $TradePrice - $Commission),'0.####') "/>
          </xsl:when>

          <xsl:when test="($Side='SL' or $Side='SS') and $Asset='Equity'">
            <xsl:value-of select="format-number(($Position * $TradePrice - $Commission - $SecFee - $NetNotional),'0.####') "/>
          </xsl:when>

          <xsl:when test="($Side='SL' or $Side='SS') and $Asset='Option'">
            <xsl:value-of select="format-number((($Position * 100) * $TradePrice  - $Commission - $SecFee - $NetNotional),'0.####')"/>
          </xsl:when>

        </xsl:choose>              
            </xsl:variable>-->

			 
            <!--<Fees>
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
            </Fees>-->

			 

			  <xsl:variable name="COL7">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL7"/>
				  </xsl:call-template>
			  </xsl:variable>


		



            <!--<SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>-->

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>


  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>