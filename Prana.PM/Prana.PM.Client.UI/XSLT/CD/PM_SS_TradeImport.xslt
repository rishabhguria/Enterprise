<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>






  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        
          
        <xsl:if test="number(COL5)">
          <xsl:if test="COL1!='li'">
            <xsl:if test="COL1!='lo'">
          <PositionMaster>

            <xsl:variable name="PB_FUND_NAME">
              <xsl:value-of select='COL12'/>
            </xsl:variable>
			
			
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='MS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <AccountName>
              <xsl:value-of select='$PRANA_FUND_NAME'/>
            </AccountName>
             

            <Symbol>
              <xsl:value-of select='translate(COL2,$varSmall,$varCapital)'/>
            </Symbol>

            <LotId>
              <xsl:value-of select='COL10'/>
            </LotId>


          
                <NetPosition>
                  <xsl:value-of select='COL5'/>
                </NetPosition>
             

            <!--Side-->

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="COL1='by'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="COL1='sl'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="COL1='ss'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="COL1='cs'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>
              </xsl:choose>
            </SideTagValue>

            <CostBasis>
             
                  <xsl:value-of select='COL17'/>
                
            </CostBasis>         

            <PositionStartDate>
              <xsl:value-of select='COL3'/>
            </PositionStartDate>
            <PositionSettlementDate>
              <xsl:value-of select='COL4'/>
            </PositionSettlementDate>
            <ExternalTransId>
              <xsl:value-of select='COL11'/>
            </ExternalTransId>
            <Commission>
              <xsl:value-of select='COL14'/>
            </Commission>
            <StampDuty>
              <xsl:value-of select='COL15'/>
            </StampDuty>
            <Fees>
              <xsl:value-of select='COL16'/>
            </Fees>


          </PositionMaster>
            </xsl:if>
          </xsl:if>
        </xsl:if>
      
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>