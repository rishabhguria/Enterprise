<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


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
	
	    <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='CALL'">
      <xsl:choose>
        <xsl:when test="$Month=01">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test=" $Month=06">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='PUT'">
      <xsl:choose>
        <xsl:when test="$Month=01">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12">
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
    <xsl:if test="contains($Symbol,'CALL') or contains($Symbol,'PUT')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="normalize-space(COL10)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDate">
        <xsl:value-of select="normalize-space(substring-before(substring-after($Symbol,' '),'@'))"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after($ExpiryDate,'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="normalize-space(substring-before($ExpiryDate,'/'))"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-after(substring-after($ExpiryDate,'/'),'/')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="normalize-space(substring-before($Symbol,' '))"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(normalize-space(substring-after($Symbol,'@')),'#.00')"/>
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

      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

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
						<xsl:with-param name="Number" select="COL31"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and $Position!='0' and COL29!='0'">
				
				<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JP Morgan'"/>
						</xsl:variable>
						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="normalize-space(COL13)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

					<xsl:variable name ="COL17">
						
						
								<xsl:value-of select="normalize-space(COL17)"/>
						
					</xsl:variable>
					
					 <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="substring-before(COL14,' ') ='CALL' or substring-before(COL14,' ') ='PUT'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
					
					
						<xsl:variable name ="Asset">
							<xsl:choose>
								<xsl:when test=" contains($COL17,'COMMON STK')">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
   <xsl:when test="substring-before(COL14,' ') ='Call' or substring-before(COL14,' ') ='Put'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
					
						<!--<xsl:variable name="Symbol">
							<xsl:value-of select="normalize-space(COL10)"/>
						</xsl:variable>-->

					<xsl:variable name="Symbol" >
						<xsl:choose>

							<xsl:when test="contains(normalize-space(COL10),' ')">
								<xsl:value-of select="concat(substring-before(normalize-space(COL10),' '),substring-after(normalize-space(COL10),' '))"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="normalize-space(COL10)"/>
							</xsl:otherwise>

						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="COL23">
						<xsl:value-of select="normalize-space(COL23)"/>
					</xsl:variable>

					<xsl:variable name="varSEDOL">
						<xsl:value-of select="normalize-space(COL8)"/>
					</xsl:variable>
					
						  <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL14"/>
                  </xsl:call-template>
                </xsl:when>
                
               
                <xsl:otherwise>
                  <xsl:value-of select="COL10"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
			
						
					
					
						
						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="translate(COL45,'$','')"/>
							</xsl:call-template>
						</xsl:variable>
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
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="translate(COL62,'$','')"/>
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
						
						<xsl:variable name="Side">
							<xsl:value-of select="normalize-space(COL12)"/>
						</xsl:variable>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='PURCHASE OPT'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL SHORT'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										
										<xsl:when test="$Side='SELL OPTION'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										
										<xsl:when test="$Side='WRITE OPTION'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										
										<xsl:when test="$Side='BUY TO CLOSE'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='BUY-BACK OPT'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='PURCHASE'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='SALE'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$Side='SELL SHORT'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										
										<xsl:when test="$Side='SHORT SALE'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="$Side='BUY TO CLOSE'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='BUY-BACK OPT'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='SHORT SALE BUYBACK'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>
						
						<xsl:variable name="varStampDuty">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>
						<FXRate>
							<xsl:choose>
								<xsl:when test ="number($varStampDuty)">
									<xsl:value-of select ="$varStampDuty"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>
						
						<FXConversionMethodOperator>
							<xsl:choose>
								<xsl:when test ="COL23='USD'">
									<xsl:value-of select ="'M'"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select ="'D'"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXConversionMethodOperator>
						
						

						<xsl:variable name="Fees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL63"/>
							</xsl:call-template>
						</xsl:variable>

					<SecFee>
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
					</SecFee>
						
					<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL48)"/>
						</xsl:variable>
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>
						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								
								<xsl:when test="normalize-space(COL48)='J.P. MORGAN SECURITIES LLC/JPMC'">
									<xsl:value-of select="'100'"/>
								</xsl:when>
								<xsl:when test="normalize-space(COL48)='MORGAN STANLEY SMITH BARNEY LLC'">
									<xsl:value-of select="'99'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

		

			 <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL29,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL29,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL29,1,4)"/>
            </xsl:variable>
           

					<PositionStartDate>
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</PositionStartDate>

						<xsl:variable name="varSDay">
              <xsl:value-of select="substring(COL30,7,2)"/>
            </xsl:variable>

            <xsl:variable name="varSMonth">
              <xsl:value-of select="substring(COL30,5,2)"/>
            </xsl:variable>

            <xsl:variable name="varSYear">
              <xsl:value-of select="substring(COL30,1,4)"/>
            </xsl:variable>
						
						<PositionSettlementDate>
							<xsl:value-of select="concat($varSMonth,'/',$varSDay,'/',$varSYear)"/>
						</PositionSettlementDate>
						
						<PBSymbol>
							<xsl:value-of select="$PB_COMPANY_NAME"/>
						</PBSymbol>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>	
</xsl:stylesheet>


