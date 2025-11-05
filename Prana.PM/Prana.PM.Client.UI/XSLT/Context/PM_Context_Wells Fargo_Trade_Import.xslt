<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>
	<xsl:template name="Date">
		<xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="1"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="2"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="3"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="4"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="5"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="7"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="8"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="9"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>



  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL34"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Quantity)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'WellsFargo'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL21)"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>

            <xsl:variable name="Symbol">
				<xsl:value-of select="normalize-space(COL15)"/>

			</xsl:variable>

			<Symbol>

				<xsl:choose>				

                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>


            </Symbol>          


            <xsl:variable name="PB_FUND_NAME" select="COL7"/>

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

					  <xsl:when test="$Quantity &gt; 0">
						  <xsl:value-of select="$Quantity"/>
					  </xsl:when>

					  <xsl:when test="$Quantity &lt; 0">
						  <xsl:value-of select="$Quantity * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </NetPosition>


			  <xsl:variable name="PB_BROKER_NAME">
				  <xsl:value-of select="normalize-space(COL53)"/>
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


			  <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL35"/>
              </xsl:call-template>
            </xsl:variable>
			  <CostBasis>
				  <xsl:choose>
					  <xsl:when test="$AvgPrice &gt; 0">
						  <xsl:value-of select="$AvgPrice"/>

					  </xsl:when>
					  <xsl:when test="$AvgPrice &lt; 0">
						  <xsl:value-of select="$AvgPrice * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>
			  </CostBasis>


            <xsl:variable name="Side" select="COL32"/>

			  <SideTagValue>

				  <xsl:choose>
					  <xsl:when test="string-length(COL13) &gt; 20">
						  <xsl:choose>
							  <xsl:when test="$Side='Buy'">
								  <xsl:value-of select="'A'"/>
							  </xsl:when>
							  <xsl:when test="$Side='Sell'">
								  <xsl:value-of select="'D'"/>
							  </xsl:when>

							  <xsl:when test="$Side='Sell Short'">
								  <xsl:value-of select="'C'"/>
							  </xsl:when>

							  <xsl:when test="$Side='Cover Short' ">
								  <xsl:value-of select="'B'"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="$Side='Buy'">
								  <xsl:value-of select="'1'"/>
							  </xsl:when>
							  <xsl:when test="$Side='Sell'">
								  <xsl:value-of select="'2'"/>
							  </xsl:when>

							  <xsl:when test="$Side='Sell Short'">
								  <xsl:value-of select="'5'"/>
							  </xsl:when>

							  <xsl:when test="$Side='Cover Short' ">
								  <xsl:value-of select="'B'"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>

						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>
				  
			</SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>			

			  <PositionStartDate>
				  <xsl:value-of select="COL26"/>
			  </PositionStartDate>

			  <PositionSettlementDate>
				  <xsl:value-of select="COL27"/>				 
			  </PositionSettlementDate>


             <xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL43"/>
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


            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL41"/>
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

            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL42"/>
              </xsl:call-template>
            </xsl:variable>
			  <SecFee>
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
            </SecFee>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>