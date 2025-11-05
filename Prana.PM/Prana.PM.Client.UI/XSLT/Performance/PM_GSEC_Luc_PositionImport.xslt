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
    <xsl:if test="COL2='OPTION'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),6,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),4,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),2,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),8,1)"/>
      </xsl:variable>

      <xsl:variable name="Decimal">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),13),'##')"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(concat(substring(substring-after($Symbol,$UnderlyingSymbol),9,5),'.',substring(substring-after($Symbol,$UnderlyingSymbol),13)),'#.00')"/>
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
			<xsl:for-each select="//PositionMaster">
				<xsl:if test="normalize-space(COL2)!='Symbol' and number(COL8) and normalize-space(COL6)!='Cash and Equivalents'">
					<PositionMaster>
						<!--   Fund -->
						<PositionStartDate>
							<xsl:value-of select="COL7"/>
						</PositionStartDate>

						<PBSymbol>
							<xsl:value-of select="COL6"/>
						</PBSymbol>

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GSec']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSec']/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SymbolCurrency_NAME" >
							<xsl:value-of select="COL6"/>
						</xsl:variable>

						<xsl:variable name = "PB_Currency_NAME" >
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SymbolCurrency_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GSec']/SymbolData[@PBCompanyName=$PB_SymbolCurrency_NAME and @Currency=$PB_Currency_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<xsl:value-of select="$PB_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="varMonthNo" select="substring(COL17,9,2)"/>
						<xsl:variable name="varYearNo" select="substring(COL17,7,2)"/>
						<xsl:variable name="varSymbol" select="normalize-space(substring(COL17,1,6))"/>
						<xsl:variable name="varStrikePrice" select="format-number(concat(substring(COL17,14,5),'.',substring(COL17,19)),'#.00')"/>
						<xsl:variable name="varCallPutCode" select="substring(COL17,13,1)"/>
						<xsl:variable name = "varMonthCode" >
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="$varMonthNo" />
								<xsl:with-param name="varPutCall" select="$varCallPutCode"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="concat('O:',$varSymbol,' ',$varYearNo,$varMonthCode,$varStrikePrice)"/>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="normalize-space(COL2)='OPTION'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL31)"/>
                  </xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="normalize-space(COL6)=''">
											<xsl:choose>
												<xsl:when test="$PRANA_SymbolCurrency_NAME=''">
													<xsl:value-of select="$PB_SymbolCurrency_NAME"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$PRANA_Symbol_NAME=''">
													<xsl:value-of select="COL3"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$PRANA_Symbol_NAME"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test="normalize-space(COL2)='OPTION'">
									<xsl:value-of select="concat(COL31,'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>

						<!--<Bloomberg>
							<xsl:value-of select ="concat(COL5, ' EQUITY')"/>
						</Bloomberg>-->


						<CostBasis>
							<xsl:choose>
								<xsl:when test="number(COL15)">
									<xsl:value-of select="COL15"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<NetPosition>
							<xsl:choose>
								<xsl:when test="COL8 &gt; 0">
									<xsl:value-of select="COL8"/>
								</xsl:when>
								<xsl:when test="COL8 &lt; 0">
									<xsl:value-of select="COL8*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL8 &gt; 0 ">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL8 &lt; 0">
									<xsl:value-of select="5"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CounterPartyID>
							<xsl:value-of select ="16"/>
						</CounterPartyID>

						<Description>
							<xsl:value-of select="COL4"/>
						</Description>

						<FXRate>
							<xsl:value-of select ="COL16"/>
						</FXRate>

						<FXConversionMethodOperator>
							<xsl:value-of select ="'M'"/>
						</FXConversionMethodOperator>

						<!--<SMMappingReq>
							<xsl:value-of select="'SecMasterMapping.xml'"/>
						</SMMappingReq>-->

						<xsl:variable name="varOption">
							<xsl:call-template name="Option">
								<xsl:with-param name="Symbol" select="normalize-space(COL31)"/>
							</xsl:call-template>
						</xsl:variable>

						<TradeAttribute1>
							<xsl:choose>
								<xsl:when test="contains(translate(COL2,$upper_CONST,$lower_CONST),'swap')">
									<xsl:choose>
										<xsl:when test="normalize-space(COL2)='OPTION'">
											<xsl:value-of select="translate(concat($varOption,'_swap'),$upper_CONST,$lower_CONST)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="normalize-space(COL6)=''">
													<xsl:choose>
														<xsl:when test="$PRANA_SymbolCurrency_NAME=''">
															<xsl:value-of select="translate(concat($PB_SymbolCurrency_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate(concat($PRANA_SymbolCurrency_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>
													<xsl:choose>
														<xsl:when test="$PRANA_Symbol_NAME=''">
															<xsl:value-of select="translate(concat(COL3,'_swap'),$upper_CONST,$lower_CONST)"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate(concat($PRANA_Symbol_NAME,'_swap'),$upper_CONST,$lower_CONST)"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="normalize-space(COL2)='OPTION'">
											<xsl:value-of select="$varOption"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="normalize-space(COL6)=''">
													<xsl:choose>
														<xsl:when test="$PRANA_SymbolCurrency_NAME=''">
															<xsl:value-of select="$PB_SymbolCurrency_NAME"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="$PRANA_SymbolCurrency_NAME"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>
													<xsl:choose>
														<xsl:when test="$PRANA_Symbol_NAME=''">
															<xsl:value-of select="COL3"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="$PRANA_Symbol_NAME"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</TradeAttribute1>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>