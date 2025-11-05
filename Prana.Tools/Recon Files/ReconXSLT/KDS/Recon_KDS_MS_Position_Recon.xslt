<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL12"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL29)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>



            <xsl:variable name="Asset">
              <xsl:choose>
                <xsl:when test="COL34='FUTURE'">
                  <xsl:value-of select="'Future'"/>
                </xsl:when>
				  
				  <xsl:when test="COL34='OPTION'">
					  <xsl:value-of select="'Option'"/>
				  </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


			  <xsl:variable name="Currency">
				  <xsl:value-of select="substring-after(substring-after(substring-after(translate(COL49,$lower_CONST,$upper_CONST),' '),' '),' ')"/>
			  </xsl:variable>
			  
            <xsl:variable name="Symbol">
              <xsl:choose>
                <xsl:when test="string-length(COL38)=4">
                  <xsl:value-of select="concat(substring(COL38,1,2),' ',substring(COL38,3,4))"/>
                </xsl:when>

                <xsl:when test="string-length(COL38)=5">
                  <xsl:value-of select="concat(substring(COL38,1,3),' ',substring(COL38,4,5))"/>
                </xsl:when>
                
              </xsl:choose>
            </xsl:variable>

			  <xsl:variable name="Bloomberg">
				  <xsl:choose>
					  <!--<xsl:when test="contains(COL39,'ELEC')">
						  <xsl:value-of select="normalize-space(translate(COL39,'ELEC',''))"/>
					  </xsl:when>-->

					  <xsl:when test="contains(substring-before(substring-after(COL39,' '),' '),'ELEC')">
						  <xsl:value-of select="translate(concat(substring-before(COL39,' '),' ',substring-after(substring-after(COL39,' '),' ')),$lower_CONST,$upper_CONST)"/>
					  </xsl:when>
					  <xsl:when test="contains(substring-before(substring-after(substring-after(COL39,' '),' '),' '),'ELEC')">
						  <xsl:value-of select="translate(concat(substring-before(COL39,' '),' ',substring-before(substring-after(COL39,' '),' '),' ',substring-after(substring-after(substring-after(COL39,' '),' '),' ')),$lower_CONST,$upper_CONST)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="translate(COL39,$lower_CONST,$upper_CONST)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

				
                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="$Bloomberg"/>
                </xsl:when>

				  <!--xsl:when test="$Asset='Option'">
					  <xsl:value-of select="concat(substring-before(COL39,' '),' ',substring-before(substring-after(COL39,' '),' '),' ',$Currency)"/>
				  </xsl:when>-->


				  <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>


            </Symbol>

            <xsl:variable name ="Date" select="COL5"/>


            <xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>

            <TradeDate>
              <xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>

            </TradeDate>

            <xsl:variable name ="Date1" select="COL1"/>


            <xsl:variable name="Year" select="substring($Date1,1,4)"/>
            <xsl:variable name="Month1" select="substring($Date1,5,2)"/>
            <xsl:variable name="Day1" select="substring($Date1,7,2)"/>

            <SettlementDate>
              <xsl:value-of select="concat($Month1,'/',$Day1,'/',$Year)"/>

            </SettlementDate>


			  <Multiplier>
				  <xsl:value-of select="COL42"/>
			  </Multiplier>


			  <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="string-length(COL41)=21">
                  <xsl:value-of select="concat(COL41,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>-->


            <xsl:variable name="PB_FUND_NAME" select="COL4"/>

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

			  <xsl:variable name="Side" select="normalize-space(COL13)"/>


			  <Quantity>
              <xsl:choose>
                <xsl:when test="$Side='L'">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>

				  <xsl:when test="$Side='S'">
					  <xsl:value-of select="$Quantity  * -1"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>


            <xsl:variable name="MarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test="$MarkPrice &gt; 0">
                  <xsl:value-of select="$MarkPrice"/>

                </xsl:when>
                <xsl:when test="$MarkPrice &lt; 0">
                  <xsl:value-of select="$MarkPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </MarkPrice>


           

            <Side>

              <xsl:choose>

                <xsl:when test="$Asset='Future'">
                  <xsl:choose>
                    <xsl:when test="$Side='L'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>

                    <xsl:when test="$Side='S'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>

                  
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>



                <xsl:otherwise>


                  <xsl:choose>
                    <xsl:when test="$Side='L'">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>

                    <xsl:when test="$Side='S'">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>

                   
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>


                </xsl:otherwise>

              </xsl:choose>




            </Side>
			 
			  <Bloomberg>
				  <xsl:value-of select="''"/>
			  </Bloomberg>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>



            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL23"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>
				<xsl:choose>
					<xsl:when test="COL24='DR'">
						<xsl:value-of select="-1*$MarketValue"/>
					</xsl:when>
					<xsl:when test="COL24=''">
						<xsl:value-of select="$MarketValue"/>
					</xsl:when>
					
					<!--<xsl:when test="$Side='L'">
						<xsl:value-of select="$MarketValue"/>
					</xsl:when>-->

					<!--<xsl:when test="$Side='S'">
						<xsl:value-of select="$MarketValue"/>
					</xsl:when>-->
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
            </MarketValue>

         


            <CurrencySymbol>
              <xsl:value-of select="normalize-space(COL14)"/>
            </CurrencySymbol>

			  <SettlCurrency>
				  <xsl:value-of select="normalize-space(COL14)"/>
			  </SettlCurrency>


            <CUSIP>
              <xsl:value-of select="COL29"/>
            </CUSIP>



			  <SMRequest>
				  <xsl:value-of select="'true'"/>
			  </SMRequest>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>