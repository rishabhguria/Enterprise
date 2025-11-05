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

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>
  
  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month='Jul' ">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL16"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and COL7!='Cash' and COL8!='Deposit' and COL8!='Interest' and COL8!='Withdraw'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>
						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="COL12"/>
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
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>					
						<xsl:variable name ="Asset">
							<xsl:choose>
								<xsl:when test=" COL7='Option' and string-length(COL10 &gt; 20)">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>							
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="Symbol">
							<xsl:value-of select="COL10"/>
						</xsl:variable>
            <xsl:variable name="varSEDOL">
              <xsl:value-of select="COL14"/>
            </xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
                <xsl:when test="$varSEDOL!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="''"/>
								</xsl:when>             
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COMPANY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
            <SEDOL>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSEDOL!=''">
                  <xsl:value-of select="$varSEDOL"/>
                </xsl:when>
                <xsl:when test="$Asset='EquityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>
						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
                <xsl:when test="$varSEDOL!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:value-of select="concat(COL10,'U')"/>
								</xsl:when>								
								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>
            <xsl:variable name="Side">
							  <xsl:value-of select="COL8"/>
						</xsl:variable>

            <xsl:variable name="varPrice1">
              <xsl:value-of select="(COL20 + COL18 + COL19)"/>
            </xsl:variable>

            <xsl:variable name="varPrice2">
              <xsl:value-of select="(COL20 - COL18 - COL19)"/>
            </xsl:variable>

            
            <xsl:variable name="varCostBasis">
              <xsl:choose>
                <xsl:when test="$Asset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$Side='Sell'">
                      <xsl:value-of select="$varPrice1 div COL16"/>
                    </xsl:when>
                    <xsl:when test="$Side='Buy'">
                      <xsl:value-of select=" $varPrice2 div COL16"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Side='Sell'">
                      <xsl:value-of select="$varPrice1 div COL16"/>
                    </xsl:when>
                    <xsl:when test="$Side='Buy'">
                      <xsl:value-of select=" $varPrice2 div COL16"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            
						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varCostBasis"/>
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
						

						<xsl:variable name="COL32">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="translate(COL32,'$','')"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="Fees">
							<xsl:choose>
								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$COL32"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
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
								<xsl:with-param name="Number" select="translate(COL18,'$','')"/>
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
						
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side='Sell'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="$Side='BC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
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
										<xsl:when test="$Side='SellShort'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="$Side='CoverShort'">
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
								<xsl:with-param name="Number" select="translate(COL19,'$','')"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
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
						</SecFee>
						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="COL15"/>
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
            <xsl:variable name="varPDay">
              <xsl:value-of select="substring-before(COL4,'-')"/>
            </xsl:variable>
            <xsl:variable name="varPMonth">
              <xsl:call-template name="MonthCodevar">
                <xsl:with-param name="Month" select="substring-before(substring-after(COL4,'-'),'-')"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varPYear">
              <xsl:value-of select="substring-after(substring-after(COL4,'-'),'-')"/>
            </xsl:variable>
						<PositionStartDate>
              <xsl:value-of select="concat($varPMonth,'/',$varPDay,'/',$varPYear)"/>
						</PositionStartDate>
            <xsl:variable name="varSDay">
              <xsl:value-of select="substring-before(COL5,'-')"/>
            </xsl:variable>
            <xsl:variable name="varSMonth">
              <xsl:call-template name="MonthCodevar">
                <xsl:with-param name="Month" select="substring-before(substring-after(COL5,'-'),'-')"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="varSYear">
              <xsl:value-of select="substring-after(substring-after(COL5,'-'),'-')"/>
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


