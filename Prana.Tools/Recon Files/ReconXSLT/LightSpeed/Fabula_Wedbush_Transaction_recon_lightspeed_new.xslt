<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	
	
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


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
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
		<xsl:if test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(COL18,' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),5,2)"/>
			</xsl:variable>
			
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),3,2)"/>			
			</xsl:variable>
			
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(normalize-space(COL5),' '),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL5),' '),8) div 1000,'0.00')"/>				
			</xsl:variable>


			<xsl:variable name="MonthCodVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="$ExpiryMonth"/>
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
			
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
			
		</xsl:if>
	</xsl:template>


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
	</xsl:template>
  <xsl:template match="/">
    
    <DocumentElement>
		
      <xsl:for-each select="//Comparision">
		  <xsl:variable name="Position">
			  <xsl:call-template name="Translate">
				  <xsl:with-param name="Number" select="COL4"/>
			  </xsl:call-template>
		  </xsl:variable>
		  <xsl:if test="number($Position) and COL23='0'">
			  <PositionMaster>

			  <xsl:variable name = "PB_FUND_NAME" >
				  <xsl:value-of select="COL2"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_FUND_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Wedbush']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>

			  <AccountName>
				  <xsl:choose>
					  <xsl:when test="$PRANA_FUND_NAME!=''">
						  <xsl:value-of select="$PRANA_FUND_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select='$PB_FUND_NAME'/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </AccountName>

			  <xsl:variable name = "PB_COMPANY" >
				  <xsl:value-of select="normalize-space(COL18)"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">
				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Wedbush']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>
  
   
			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="substring-before(COL18,' ')='C' or substring-before(COL18,' ')='P'">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <xsl:variable name="Symbol">
				  <xsl:value-of select="normalize-space(COL5)"/>
			  </xsl:variable>


			  <Symbol>
				  <xsl:choose>
					  <xsl:when test ="$PRANA_SYMBOL != ''">
						  <xsl:value-of select ="$PRANA_SYMBOL"/>
					  </xsl:when>

					  <xsl:when test="$Asset='EquityOption'">
						  <xsl:call-template name="Option">
							  <xsl:with-param name="Symbol" select="COL5"/>
							  <xsl:with-param name="Suffix" select="''"/>
						  </xsl:call-template>
					  </xsl:when>

					  <xsl:when test="$Symbol!=''">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="$PB_COMPANY"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbol>

			  <Side>
				  <xsl:choose>
					  <xsl:when test="$Asset='EquityOption'">
						  <xsl:choose>
							  <xsl:when test="normalize-space(COL40)='BC'">
								  <xsl:value-of select="'Buy to Close'"/>
							  </xsl:when>
                
							  <xsl:when test="normalize-space(COL40)='BO'">
								  <xsl:value-of select="'Buy to Open'"/>
							  </xsl:when>
                
                <xsl:when test="normalize-space(COL40)='SC'">
								  <xsl:value-of select="'Sell to Close'"/>
							  </xsl:when>
                
							  <xsl:when test="normalize-space(COL40)='SO'">
								  <xsl:value-of select="'Sell to Open'"/>
							  </xsl:when>

						  </xsl:choose>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:choose>
							  <xsl:when test="COL3 = 'P' and  COL34='2'">
								  <xsl:value-of select="'Buy'"/>
							  </xsl:when>
							  <xsl:when test="COL3 = 'P' and COL34='3'">
								  <xsl:value-of select="'Buy to Close'"/>
							  </xsl:when>

							  <xsl:when test="COL3 = 'S' and COL34='2'">
								  <xsl:value-of select="'Sell'"/>
							  </xsl:when>
							  <xsl:when test="COL3 = 'S' and COL34='3'">
								  <xsl:value-of select="'Sell short'"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="''"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:otherwise>
				  </xsl:choose>


			  </Side>


			  <xsl:variable name ="varCommission">
				  <xsl:choose>
					  <xsl:when test ="number(COL15) ">
						  <xsl:value-of select ="COL15"/>
					  </xsl:when>


					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name ="varStampDuty">
				  <xsl:choose>
					  <xsl:when test ="number(COL12) ">
						  <xsl:value-of select ="COL12"/>
					  </xsl:when>


					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <Commission>
				  <xsl:choose>
					  <xsl:when test ="number($varCommission)">
						  <xsl:value-of select ="$varCommission"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </Commission>

			 

			  <StampDuty>
				  <xsl:choose>
					  <xsl:when test ="number($varStampDuty) and $varStampDuty &gt; 0">
						  <xsl:value-of select ="$varStampDuty"/>
					  </xsl:when>
					  <xsl:when test ="number($varStampDuty) and $varStampDuty &lt; 0">
						  <xsl:value-of select ="$varStampDuty * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
				
			  </StampDuty>


			  <Quantity>
				<xsl:choose>
					<xsl:when test ="number(normalize-space(COL4)) and COL4 &gt; 0">
						<xsl:value-of select="COL4"/>
					</xsl:when>
					<xsl:when test ="number(normalize-space(COL4)) and COL4 &lt; 0">
						<xsl:value-of select="COL4 * (-1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="0"/>
					</xsl:otherwise>
				</xsl:choose>
            </Quantity>

            <AvgPX>
              <xsl:choose>
                <xsl:when test="number(COL6)">
                  <xsl:value-of select="COL6"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>

            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test ="COL17 &lt; 0">
                  <xsl:value-of select="COL17*(-1)"/>
                </xsl:when>
                <xsl:when test ="COL17 &gt; 0">
                  <xsl:value-of select="COL17"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

          


            <xsl:variable name ="varOtherFees">
              <xsl:value-of select="format-number(number(COL10)+ number(COL22),'#.##')"/>
            </xsl:variable>

				  <AUECFee1>
					  <xsl:choose>
						  <xsl:when test ="number($varOtherFees)">
							  <xsl:value-of select="$varOtherFees"/>
						  </xsl:when>
						 
						  <xsl:otherwise>
							  <xsl:value-of select="0"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </AUECFee1>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
